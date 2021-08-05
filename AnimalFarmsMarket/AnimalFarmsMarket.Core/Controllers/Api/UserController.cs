using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotificationService _notification;
        private readonly IFileUpload _fileUpload;
        private readonly int _perPage;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper,
            INotificationService notification, IConfiguration configuration, IFileUpload fileUpload)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _notification = notification;
            _fileUpload = fileUpload;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
        }

        [HttpGet("get-user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Id", "No id inputted");
                return BadRequest(Utilities.CreateResponse("No id found", ModelState, ""));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);

            if (!roles.Contains("Admin"))
            {
                if (currentUser.Id != id)
                {
                    ModelState.AddModelError("Id", "Do not have authorization access");
                    return Unauthorized(Utilities.CreateResponse("Unauthorized", ModelState, ""));
                }
            }

            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
            var userRole = await _userManager.GetRolesAsync(user);
            if (user == null)
            {
                ModelState.AddModelError("Id", "Id does not exist");
                return BadRequest(Utilities.CreateResponse("Invalid Id", ModelState, ""));
            }

            var res = _mapper.Map<UserToReturnDto>(user);
            res.Roles = userRole;
            res.City = user.Address.City;
            res.State = user.Address.State;
            res.Street = user.Address.Street;

            //return the result
            return Ok(Utilities.CreateResponse("User Details", null, res));
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Admin"))
            {
                if (currentUser.Email != email)
                {
                    ModelState.AddModelError("Email", "Access is denied");
                    return BadRequest(Utilities.CreateResponse("Access denied", ModelState, ""));
                }
            }

            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email does not exist");
                return NotFound(Utilities.CreateResponse("Email does not exist", ModelState, ""));
            }

            var mappedUser = _mapper.Map<UserToReturnDto>(user);
            mappedUser.Roles = await _userManager.GetRolesAsync(user);
            mappedUser.Street = user.Address.Street;
            mappedUser.City = user.Address.City;
            mappedUser.State = user.Address.State;

            return Ok(Utilities.CreateResponse("User successfully found", null, mappedUser));
        }

        /// <summary>
        /// Change User Password
        /// First Confirm if the user enters the current password
        /// </summary>
        /// <param name="changePassword"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("update-password/{id}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!currentUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (currentUser.Id != id)
            {
                ModelState.AddModelError("Email", "Access is denied");
                return BadRequest(Utilities.CreateResponse("Access denied", ModelState, ""));
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ModelState.AddModelError("Id", "Id does not exist");
                return NotFound(Utilities.CreateResponse("Required user not found", ModelState, ""));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse("Model state error", ModelState, ""));
            }

            if (model.OldPassword == model.NewPassword)
            {
                return BadRequest(Utilities.CreateResponse("Old and new password cannot be the same", ModelState, ""));
            }

            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                var msg = Utilities.CreateResponse("Password does not match\nPlease enter your current password", null, "");
                return NotFound(msg);
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                var msg = Utilities.CreateResponse("Password does not match\nPlease enter your current password", ModelState, "");
                return BadRequest(msg);
            }

            return Ok(Utilities.CreateResponse("Password successfully changed", null, ""));
        }

        /// <summary>
        /// To delete a user, set the IsActive and EmailConfirmed properties to false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("delete-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ModelState.AddModelError("Id", "Id does not exist");
                return NotFound(Utilities.CreateResponse("Required user not found", ModelState, ""));
            }

            if (!user.IsActive && !user.EmailConfirmed)
            {
                return BadRequest(Utilities.CreateResponse("User is already deactivated", null, ""));
            }

            user.IsActive = false;
            user.EmailConfirmed = false;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                var msg = Utilities.CreateResponse("Internal Server Error", ModelState, "");
                return BadRequest(msg);
            }

            return Ok(Utilities.CreateResponse("Account successfully deleted", null, ""));
        }

        // Adding a delivery person
        [HttpPost("add-delivery-person")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDeliveryPerson([FromBody] RegisterDeliveryPersonDto model)
        {
            //Check for valid model state
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid Model state");
                return BadRequest(Utilities.CreateResponse("Invalid Data set", ModelState, ""));
            }

            //Check for an existing user with the email
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                ModelState.AddModelError("Email", "Email already exist");
                return BadRequest(Utilities.CreateResponse("Email exist", ModelState, ""));
            }

            //Check if Delivery role exist
            if (await _roleManager.FindByNameAsync("Delivery") == null)
            {
                // Create role if it doesn't exist
                await _roleManager.CreateAsync(new IdentityRole("Delivery"));
            }

            //Map the model to app user
            var deliveryPerson = new DeliveryPerson
            {
                Coverage = model.Coverage,
                CoverageLocation = model.CoverageLocation,
                NIN = model.NIN,
                NINTrackingId = model.NINTrackingId
            };

            var address = new Address
            {
                Street = model.Street,
                City = model.City,
                State = model.State,
                // Country = model.Country
            };

            var user = _mapper.Map<AppUser>(model);
            user.DeliveryPerson = deliveryPerson;
            user.Address = address;

            var userIsCreated = await _userManager.CreateAsync(user, model.Password);
            if (!userIsCreated.Succeeded)
            {
                foreach (var err in userIsCreated.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return BadRequest(Utilities.CreateResponse("User could not be created, Try Again!", ModelState, ""));
            }

            //Add user to the delivery role
            var addRole = await _userManager.AddToRoleAsync(user, "Delivery");

            if (!addRole.Succeeded)
            {
                foreach (var err in addRole.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return BadRequest(Utilities.CreateResponse("User could not be added to role, Try Again!", ModelState, ""));
            }

            //Generate email token and send confirmation token to user
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var queryParams = new Dictionary<string, string>
            {
                ["Email"] = user.Email,
                ["Token"] = token
            };

            var ss = NotificationsHelper.EmailHtmlStringTemplate($"{user.FirstName} {user.LastName}", "Account/ConfirmEmail", queryParams, "emailConfirmTemplate.html", HttpContext);
            await _notification.SendEmailAsync(user.Email, "Email Confirmation", ss, "");

            return StatusCode(201, Utilities.CreateResponse("success", null, user.Id));
        }

        [HttpPut("update-delivery-person/{id}")]
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> UpdateDeliveryPerson([FromBody] UpdateDeliveryPersonDto model, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!currentUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Id", "No id inputted");
                return BadRequest(Utilities.CreateResponse("No id found", ModelState, ""));
            }

            var roles = await _userManager.GetRolesAsync(currentUser);

            if (currentUser.Id != id)
            {
                ModelState.AddModelError("Id", "Do not have authorization access");
                return Unauthorized(Utilities.CreateResponse("Unauthorized", ModelState, ""));
            }

            //check if user exist
            var user = await _userManager.Users.Include(x => x.DeliveryPerson).Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                ModelState.AddModelError("Error", "User does not exist");
                return BadRequest(Utilities.CreateResponse("User Does not exist", ModelState, ""));
            }

            //map user detail with the update
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address.City = model.City;
            user.Address.State = model.State;
            user.Address.Street = model.Street;
            //user.Address.Country = model.Country;
            user.DeliveryPerson.Coverage = model.Coverage;
            user.DeliveryPerson.CoverageLocation = model.CoverageLocation;
            user.ZipCode = model.ZipCode;
            user.Gender = model.Gender;

            var userIsUpdated = await _userManager.UpdateAsync(user);
            if (!userIsUpdated.Succeeded)
            {
                foreach (var err in userIsUpdated.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return BadRequest(Utilities.CreateResponse("User could not be updated, Try again!", ModelState, ""));
            }

            return StatusCode(200, Utilities.CreateResponse<string>("User successfully updated", null, ""));
        }

        [HttpPost]
        [Route("add-agent")]
        public async Task<IActionResult> RegisterAgent([FromBody]RegisterAgentDto model)
        {
            //Check ModelState
            if (!ModelState.IsValid)
                return BadRequest(Utilities.CreateResponse("Model state error", errs: ModelState, data: ""));

            //Validate NIN
            if (!Utilities.ValidNIN(model.NIN))
            {
                ModelState.AddModelError("", "NIN must contain numbers only");
                return BadRequest(Utilities.CreateResponse("Invalid NIN", errs: ModelState, data: ""));
            }

            //check if user exists by email
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                ModelState.AddModelError("", "User already exist");
                return BadRequest(Utilities.CreateResponse("Invalid Credentials", errs: ModelState, data: ""));
            }

            //check if user exists by Username
            var currentUser = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName);
            if (currentUser != null)
            {
                ModelState.AddModelError("Username", "Username is taken");
                return BadRequest(Utilities.CreateResponse("Invalid username", errs: ModelState, data: ""));
            }

            var agent = new Agent
            {
                NIN = model.NIN,
                NINTrackingId = model.NINTrackingId,
                BusinessLocation = model.BusinessLocation,
                Bank = model.Bank,
                AccountName = model.AccountName,
                AccountNumber = model.AccountNumber
            };

            var user = _mapper.Map<AppUser>(model);
            user.Agent = agent;
            user.IsActive = false;

            //create user
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(err.Code, err.Description);

                return BadRequest(Utilities.CreateResponse("Something went wrong", errs: ModelState, data: ""));
            }

            //Add user to role
            await _userManager.AddToRoleAsync(user, "Agent");

            //generate token and send mail
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var queryParams = new Dictionary<string, string>()
            {
                ["email"] = user.Email,
                ["token"] = token
            };
            var template = NotificationsHelper.EmailHtmlStringTemplate($"{user.FirstName} {user.LastName}", "Account/ConfirmEmail", queryParams, "emailConfirmTemplate.html", HttpContext);

            var emailSent = await _notification.SendEmailAsync(user.Email, "Email Confirmation", template, "");

            if (!emailSent)
            {
                return BadRequest(Utilities.CreateResponse("Could not send Mail", null, data: ""));
            }

            return StatusCode(201, Utilities.CreateResponse("Registration Successful. Please check your Email for a verification", null, user.Id));
        }

        [HttpPut]
        [Route("update-agent/{id}")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> UpdateAgent(UpdateAgentDto model, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!currentUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (!ModelState.IsValid)
            {
                var msg = Utilities.CreateResponse("Model state error", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            if (currentUser.Id != id)
            {
                ModelState.AddModelError("User", "Cannot Update this User");
                return Unauthorized(Utilities.CreateResponse("Access denied", errs: ModelState, data: ""));
            }
            var user = _userManager.Users.Where(user => user.Id == id).Include(u => u.Agent).Include(u => u.Address).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("User", "User does not exist");
                return BadRequest(Utilities.CreateResponse("Invalid Credentials", errs: ModelState, data: ""));
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Agent.BusinessLocation = model.BusinessLocation;
            user.Address.Street = model.Address.Street;
            user.Address.City = model.Address.City;
            user.Address.State = model.Address.State;
            user.Gender = model.Gender;
            user.ZipCode = model.ZipCode;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);

                return BadRequest(Utilities.CreateResponse("Something went wrong", errs: ModelState, data: ""));
            }

            return NoContent();
        }

        [HttpPut]
        [Route("update-photo/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPhoto([FromForm] UpdateUserPhotoDto model, string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!currentUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (currentUser.Id != id)
            {
                ModelState.AddModelError("User", "Cannot Update this User");
                return Unauthorized(Utilities.CreateResponse("Acess denied", errs: ModelState, data: ""));
            }
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ModelState.AddModelError("User", "User does not exist");
                return BadRequest(Utilities.CreateResponse("Invalid Credentials", errs: ModelState, data: ""));
            }

            var response = _fileUpload.UploadAvatar(model.Photo);

            if (response == null)
            {
                ModelState.AddModelError("Upload", "Something went wrong");
                return BadRequest(Utilities.CreateResponse("Something went wrong", errs: ModelState, data: ""));
            }

            user.Photo = response.AvatarUrl;
            user.PublicId = response.PublicId;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return BadRequest(Utilities.CreateResponse("Something went wrong", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse("Photo successfully edited!", errs: null, data: response));
        }

        [HttpPut("update-user/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!currentUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (currentUser.Id != id)
            {
                ModelState.AddModelError("User", "Cannot Update this User");
                return Unauthorized(Utilities.CreateResponse("Acess denied", errs: ModelState, data: ""));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Id", "Id must be provided");
                var msg = Utilities.CreateResponse(message: "Id not provided", errs: ModelState, data: "");
                return BadRequest(msg);
            }
            if (id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            if (!ModelState.IsValid)
            {
                var msg = Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: "");
                return BadRequest(msg);
            }
            var user = await _userManager.Users.Include(a => a.Address).FirstOrDefaultAsync(a => a.Id == id);
            if (user == null)
            {
                ModelState.AddModelError("User", "User does not exist");
                var msg = Utilities.CreateResponse(message: "User Not Found", errs: ModelState, data: "");
                return NotFound(msg);
            }
            //Mapping
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address.City = model.Address.City;
            user.Address.Street = model.Address.Street;
            user.Address.State = model.Address.State;
            user.ZipCode = model.ZipCode;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                var msg = Utilities.CreateResponse(message: "Update not successful!", errs: ModelState, data: "");
                return BadRequest(msg);
            }
            var response = Utilities.CreateResponse(message: "update was successful", errs: null, data: "");
            return StatusCode(204, response);
        }

        [HttpGet("get-all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] int page)
        {
            if (!ModelState.IsValid)
            {
                var msg = Utilities.CreateResponse("Model state error", ModelState, "");
                return BadRequest(msg);
            }
            page = page <= 0 ? 1 : page;
            var userList = _userManager.Users.Skip((page - 1) * _perPage).Take(_perPage);
            var usersToDisplay = new List<UserDto>();

            foreach (var user in userList)
            {
                var role = await _userManager.GetRolesAsync(user);
                var userToReturn = _mapper.Map<UserDto>(user);
                userToReturn.Roles = role;
                usersToDisplay.Add(userToReturn);
            }
            var pageMetaData = Utilities.Paginate(page, _perPage, _userManager.Users.Count());
            var result = new PaginatedResultDto<UserDto>
            {
                PageMetaData = pageMetaData,
                ResponseData = usersToDisplay
            };
            var response = Utilities.CreateResponse(message: "List of all users", errs: null, data: result);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                var msg = Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                ModelState.AddModelError("Email", "Email Already Exists...");
                var msg = Utilities.CreateResponse(message: "Invalid email", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            var newUser = _mapper.Map<AppUser>(model);
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                var msg = Utilities.CreateResponse(message: "Registration not successful!", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            var role = await _roleManager.RoleExistsAsync("Customer");
            if (!role)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Customer" });
            }

            await _userManager.AddToRoleAsync(newUser, "Customer");

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var queryParams = new Dictionary<string, string>();
            queryParams["Email"] = newUser.Email;
            queryParams["Token"] = emailConfirmationToken;

            var template = NotificationsHelper.EmailHtmlStringTemplate($"{newUser.FirstName} {newUser.LastName}", "Account/ConfirmEmail", queryParams, "emailConfirmTemplate.html", HttpContext);
            await _notification.SendEmailAsync(newUser.Email, "Email Confirmation", template, "");
            var response = Utilities.CreateResponse(
                message: "successfully registered and verification mail sent",
                errs: null,
                data: $"{newUser.Id}"
            );
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-agentid")]
        public async Task<IActionResult> GetAgentId([FromQuery] string userid)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("Agent"))
            {
                if (currentUser.Id != userid)
                {
                    ModelState.AddModelError("Email", "Access is denied");
                    return BadRequest(Utilities.CreateResponse("Access denied", ModelState, ""));
                }
            }

            var user = await _userManager.Users.Include(x => x.Agent).FirstOrDefaultAsync(x => x.Agent.AppUserId == userid);

            if (user == null)
            {
                ModelState.AddModelError("user", "User does not exist");
                return NotFound(Utilities.CreateResponse("User does not exist", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse("User successfully found", null, user.Agent.Id));
        }
    }
}
