using System;
using System.Threading.Tasks;

using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Enum;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.ViewModels;

using AutoMapper;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace AnimalFarmsMarket.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private string _newUserId = "";
        private string _newUserToken = "";
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.LoginErrMsg = false;

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Token")))
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginErrMsg = false;

            if (!ModelState.IsValid) return View(model);

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseBody, response) = await HttpHelper.PostContentAsync(baseUrl, model, "api/v1/Auth/Login");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.LoginErrMsg = true;
                var errorResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(responseBody);

                foreach (var error in errorResponse.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(model);
            }

            var successResponse = JsonConvert.DeserializeObject<ResponseDto<LoginResponseDto>>(responseBody);
            var userId = successResponse.Data.UserId;
            var userToken = successResponse.Data.Token;
            var userRole = successResponse.Data.Role;

            var loggedInUser = await _userManager.FindByIdAsync(userId);

            HttpContext.Session.SetString("activeStatus", loggedInUser.IsActive.ToString());
            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("Token", userToken);
            HttpContext.Session.SetString("UserName", $"{loggedInUser.LastName} {loggedInUser.FirstName}");
            HttpContext.Session.SetString("UserRole", userRole);

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet("google-signin")]
        public IActionResult GoogleLogin(string returnUrl)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var url = baseUrl + $"/api/v1/Auth/external-login?controllerName=Account&actionName=ExternalCallback&returnUrl={returnUrl}";
            var properties = new AuthenticationProperties { RedirectUri = url };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> ExternalCallback(LoginResponseDto response, string returnUrl)
        {
            var userId = response.UserId;

            var loggedInUser = await _userManager.FindByIdAsync(userId);

            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("Token", response.Token);
            HttpContext.Session.SetString("UserRole", response.Role);
            HttpContext.Session.SetString("activeStatus", loggedInUser.IsActive.ToString());
            HttpContext.Session.SetString("UserName", $"{loggedInUser.LastName} {loggedInUser.FirstName}");

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult PasswordResetConfirm()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public IActionResult RegistrationConfirmation()
        {
            return View();
        }

        [HttpGet("account/reset-password")]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [HttpGet]
        public IActionResult AboutAgent()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AgentRegistration()
        {
            ViewBag.AgentRegistrationErr = false;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgentRegistration(AddAgentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var baseUrl = UrlHelper.BaseAddress(HttpContext);

            var agent = new RegisterAgentDto()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BusinessLocation = model.BusinessLocation,
                Password = model.Password,
                ConfirmPassword = model.Password,
                Gender = model.Gender,
                Email = model.Email,
                NIN = model.NIN,
                NINTrackingId = model.NINTrackingId,
                UserName = model.Email,
                ZipCode = model.ZipCode,
                AccountName = model.AccountName,
                AccountNumber = model.AccountNumber,
                Bank = EnumHelper.GetDisplayName(model.Bank),
                Address = new AddressToReturnDto
                {
                    Street = model.Address.Street,
                    City = model.Address.City,
                    State = EnumHelper.GetDisplayName(model.Address.State)
                    //Country = Enum.GetName(typeof(Countries), model.Address.Country)
                }
            };

            var result = await HttpHelper.PostContentAsync<RegisterAgentDto>(baseUrl, agent, "api/v1/User/add-agent");
            var content = result.Item1;
            var httpResponse = result.Item2;

            if (httpResponse.IsSuccessStatusCode)
            {
                var successResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(content);

                return RedirectToAction("RegistrationConfirmation");
            }
            else
            {
                ViewBag.AgentRegistrationErr = true;
                var DeserializedContentErrors = JsonConvert.DeserializeObject<ResponseDto<string>>(content);
                foreach (var error in DeserializedContentErrors.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //model.Token = System.Web.HttpUtility.UrlEncode(model.Token);
            if (!ModelState.IsValid)
                return View(model);
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var response = await HttpHelper.PatchContentAsync<ResetPasswordViewModel>(baseUrl, model, token, "/api/v1/auth/reset-password");
            var httpresponse = response.Item2;
            var content = response.Item1;

            if (httpresponse.IsSuccessStatusCode)
            {
                return RedirectToAction("PasswordResetConfirm");
            }
            else
            {
                var errors = JsonConvert.DeserializeObject<ResponseDto<string>>(content);
                foreach (var error in errors.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                ViewBag.Email = model.Email;
                //ViewBag.Token = System.Web.HttpUtility.UrlDecode(model.Token);
                ViewBag.Token = model.Token;
                return View(model);
            };
        }

        [HttpGet]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var modelToAdd = new RegisterUserDto()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                Password = model.Password,
                Email = model.Email,
                ConfirmPassword = model.ConfirmPassword,
                UserName = model.Email,
                ZipCode = model.ZipCode,
                Address = new Address
                {
                    City = model.Address.City,
                    State = Enum.GetName(typeof(States), model.Address.State),
                    Street = model.Address.Street
                }
            };

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var result = await HttpHelper.PostContentAsync<RegisterUserDto>(baseUrl, modelToAdd, "/api/v1/User/add-user");
            var responseObj = result.Item1;
            var response = result.Item2;
            if (!response.IsSuccessStatusCode)
            {
                var errResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(responseObj);
                foreach (var err in errResponse.Errs)
                {
                    ModelState.AddModelError(err.Key, err.Value);
                }
                return View(model);
            }
            return RedirectToAction("RegistrationConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);

            var Response = await HttpHelper.PostContentAsync<ForgotPasswordViewModel>(baseUrl, model, "/api/v1/auth/forgot-password");

            var content = Response.Item1;
            var httpResponse = Response.Item2;
            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            else
            {
                var DeserializedContentErrors = JsonConvert.DeserializeObject<ResponseDto<string>>(content);
                foreach (var error in DeserializedContentErrors.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var result = await HttpHelper.PostContentAsync(baseUrl, new { }, "api/v1/Auth/logout");
            var response = result.Item2;

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }
            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home", new { confirmation = "failed" });

            var baseUrl = UrlHelper.BaseAddress(HttpContext);

            var model = new ConfirmEmailDTO
            {
                Email = email,
                Token = token
            };

            var Response = await HttpHelper.PostContentAsync<ConfirmEmailDTO>(baseUrl, model, "/api/v1/auth/confirm-email");

            var content = Response.Item1;
            var httpResponse = Response.Item2;
            if (httpResponse.IsSuccessStatusCode)
            {
                return View("EmailConfirmation");
            }
            else
            {
                var DeserializedContentErrors = JsonConvert.DeserializeObject<ResponseDto<string>>(content);
                foreach (var error in DeserializedContentErrors.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return View("EmailConfirmation", model);
            }
        }
    }
}
