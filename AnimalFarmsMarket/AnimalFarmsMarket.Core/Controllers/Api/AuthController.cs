using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Core.Security;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AnimalFarmsMarket.Data.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IOptions<JWTData> _JWTData;
        private INotificationService _notificationService;

        public AuthController(ILogger<AuthController> logger, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IOptions<JWTData> options, INotificationService notificationService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _JWTData = options;
            _notificationService = notificationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var msg = Utilities.CreateResponse(
                                message: "Incomplete model", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Credential");
                var errMsg = Utilities.CreateResponse(message: "Invalid Credentials", errs: ModelState, data: "");
                return BadRequest(errMsg);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("Email", "Email not confirmed yet");
                var errMsg = Utilities.CreateResponse(message: "Email not confirmed", errs: ModelState, data: "");
                return BadRequest(errMsg);
            }

            var checkPassword = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!checkPassword.Succeeded)
            {
                ModelState.AddModelError("Password", "Invalid Credential");
                var errMsg = Utilities.CreateResponse(message: "Invalid Credentials", errs: ModelState, data: "");
                return BadRequest(errMsg);
            }

            var userRoles = await _userManager.GetRolesAsync(user) as List<string>;
            var token = JWTService.GenerateToken(user, userRoles, _JWTData);

            LoginResponseDto res = new LoginResponseDto
            {
                Token = token,
                Role = string.Join(",", await _userManager.GetRolesAsync(user)),
                UserId = user.Id
            };

            return Ok(Utilities.CreateResponse("Login Successful", null, res));
        }


        [HttpGet("google-signin")]
        public IActionResult LoginWithGoogle()
        {

            var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLogin") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("external-login")]
        public async Task<IActionResult> ExternalLogin([FromQuery] string controllerName, string actionName, string returnUrl)
        {

            var authenticate = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var emailClaim = authenticate.Principal.FindFirst(ClaimTypes.Email);
            var nameClaim = authenticate.Principal.FindFirst(ClaimTypes.GivenName);
            var surnameClaim = authenticate.Principal.FindFirst(ClaimTypes.Surname);

            if (emailClaim is null)
            {
                ModelState.AddModelError("Email", "Invalid Email");
                var errMsg = Utilities.CreateResponse("Invalid Email", errs: ModelState, data: "");
                return BadRequest(errMsg);

            }
            var user = await _userManager.FindByEmailAsync(emailClaim.Value);
            if (user is null)
            {
                var newUser = new AppUser
                {
                    FirstName = nameClaim.Value,
                    LastName = surnameClaim.Value,
                    Gender = 0,
                    Email = emailClaim.Value,
                    UserName = emailClaim.Value
                };
                user = await RegisterExternalUser(newUser, "Customer", Request.Scheme, Request.Host, Request.PathBase);
                if (user is null)
                {
                    ModelState.AddModelError("Registration", "Registration Failed");
                    var errMsg = Utilities.CreateResponse("Registration Failed", errs: ModelState, data: "");
                    return BadRequest(errMsg);

                }
            }

            var userRoles = await _userManager.GetRolesAsync(user) as List<string>;
            var token = JWTService.GenerateToken(user, userRoles, _JWTData);

            var res = new LoginResponseDto
            {
                Token = token,
                Role = string.Join(",", await _userManager.GetRolesAsync(user)),
                UserId = user.Id
            };

            if (!string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(actionName))
            {
                var response = new Dictionary<string, string>
                {
                    {"Token", res.Token},
                    {"Role", res.Role},
                    {"UserId", res.UserId},
                    {"returnUrl", returnUrl}
                };
                
                return RedirectToAction(actionName, controllerName, response);
            }

            return Ok(Utilities.CreateResponse("Login Successful", null, res));
        }

        private async Task<AppUser> RegisterExternalUser(AppUser user, string role, string requestScheme, HostString host, string pathBase)
        {
            var newUser = new AppUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = 0,
                Email = user.Email,
                UserName = user.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, role);
                var queryParams = new Dictionary<string, string>()
                {
                    ["email"] = user.Email,
                    ["token"] = ""
                };
              
                var template = NotificationsHelper.EmailHtmlStringTemplate($"{newUser.FirstName} {newUser.LastName}", "", queryParams, "welcomeTemplate.html", HttpContext);
                var sent = await _notificationService.SendEmailAsync(user.Email, "Welcome Message", template, "");
                return newUser;
            }
            return null;
        }


        //base_url/api/v1/auth/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                var responseObj = Utilities.CreateResponse<string>("Model errors", ModelState, "");
                return BadRequest(responseObj);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email does not exist");
                var responseObj = Utilities.CreateResponse<string>($"Invalid Email", ModelState, "");

                return BadRequest(responseObj);
            }

            //Get the password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var queryParams = new Dictionary<string, string>()
            {
                ["email"] = user.Email,
                ["token"] = token
            };
            var template = NotificationsHelper.EmailHtmlStringTemplate($"{user.FirstName} {user.LastName}", "account/reset-password", queryParams, "resetpasswordemailtemplate.html", HttpContext);

            //send password reset email to user
            var emailSent = await _notificationService.SendEmailAsync(user.Email, "Reset Password", template, "");
            if (emailSent)
            {
                var responseObj = Utilities.CreateResponse<string>($"Successfully send forgot password mail", null, "");
                return Ok(responseObj);
            }
            else
            {
                ModelState.AddModelError("EmailService", "There was an error sending the password reset link. Please try again");
                return BadRequest(Utilities.CreateResponse<string>("Service not available", ModelState, ""));
            }
        }

        //base_url/api/v1/auth/reset-password
        [HttpPatch("reset-password")]
        public async Task<ActionResult<ResponseDto<string>>> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                var responseObj = Utilities.CreateResponse<string>("Model errors", ModelState, null);
                return BadRequest(responseObj);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email does not exist");
                var responseObj = Utilities.CreateResponse<string>("Model errors", ModelState, null);
                return BadRequest(responseObj);
            }

            //var token = HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok(Utilities.CreateResponse<string>("Password reset was successful", null, null));
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return BadRequest(Utilities.CreateResponse<string>("Token", ModelState, null));
            }
        }

        //base_url/api/v1/auth/confirm-email
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO model)
        {
            if (!ModelState.IsValid)
            {
                var responseObj = Utilities.CreateResponse<string>("Model errors", ModelState, null);
                return BadRequest(responseObj);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Invalid email", $"Email does not exist");
                var responseObj = Utilities.CreateResponse<string>($"Email does not exist", ModelState, null);
                return BadRequest(responseObj);
            }


            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count < 1) {
                ModelState.AddModelError("User has no role", $"User has not been assined row");
                
                var responseObj = Utilities.CreateResponse<string>($"User has no role", ModelState, null);
                return BadRequest(responseObj);
            }


            //var token = HttpUtility.UrlDecode(model.Token);
            var emailConfirmed = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (emailConfirmed.Succeeded)
            {

                if(!userRoles.Contains("Agent"))
                {
                    user.IsActive = true;
                    var res = await _userManager.UpdateAsync(user);
                    if (!res.Succeeded)
                    {
                        ModelState.AddModelError("Failed to activate user", $"Could not activate user");

                        var responseObj = Utilities.CreateResponse<string>($"User account activation failed", ModelState, null);
                        return BadRequest(responseObj);
                    }

                }
                

                return Ok(Utilities.CreateResponse<string>("Email Successfully Verified", null, null));
            }
            else
            {
                foreach (var err in emailConfirmed.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Utilities.CreateResponse<string>("Token", ModelState, null));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return Ok();
        }

        [HttpPatch("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model, [FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id not provided", ModelState, ""));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse("Model state error", ModelState, ""));
            }

            var currentLoggedInUser = await _userManager.GetUserAsync(User);


            if (!await _userManager.IsInRoleAsync(currentLoggedInUser, "Admin"))
            {
                if (currentLoggedInUser.Id != id)
                {
                    ModelState.AddModelError("Id", "could not authorize user");
                    return Unauthorized(Utilities.CreateResponse("Id is invalid", ModelState, ""));
                }
            }

            var user = await _userManager.FindByIdAsync(id);
            var passwordExist = await _userManager.CheckPasswordAsync(user, model.OldPassword);

            if (!passwordExist)
            {
                ModelState.AddModelError("Password", "Invalid password provided");
                return BadRequest(Utilities.CreateResponse("Invalid Credential", ModelState, ""));
            }

            var res = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return BadRequest(Utilities.CreateResponse("Password", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse("Password changed successfully", null, ""));
        }

        [Authorize(Roles = "Admin, Agent")]
        [HttpPatch("activate-account/{userId}")]
        public async Task<IActionResult> ActivateAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("Invalid Id", "User Id cannot be Empty");
                return BadRequest(Utilities.CreateResponse<string>("User Id is Empty", ModelState, ""));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("NotFound", $"No Result Found For the User {userId} ");
                return NotFound(Utilities.CreateResponse<string>($"No Result Found For {userId}", ModelState, ""));
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("Not Confirmed", "User Email not Confirmed");
                return BadRequest(Utilities.CreateResponse<string>("User Email not confirmed", ModelState, ""));
            }

            if (user.IsActive)
            {
                ModelState.AddModelError("Already Active", $"User Account with {userId} is already Active");
                return BadRequest(Utilities.CreateResponse<string>($"User Account with {userId} is already Active", ModelState, ""));
            }

            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return Ok(Utilities.CreateResponse<string>("Account Activation Successful", null, ""));
        }

    }
}