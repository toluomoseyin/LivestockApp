using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Enum;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.ViewModels;

using AutoMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace AnimalFarmsMarket.Core.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DashboardController(UserManager<AppUser> userManager, IMapper mapper, IConfiguration config)
        {
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string status)
        {
            var user = _userManager.GetUserAsync(User);

            var userIsActive = user.Result.IsActive;
            if (!userIsActive)
            {
                return RedirectToAction("Profile", new { q = "update-profile" });
            }

            if (status != null)
            {
                ViewData["value"] = "paid";
            }

            var model = new DashboardViewModel();

            var invoices = await FetchUserInvoices();
            var payments = await FetchUserPaymentHistory();
            var orders = await FetchUserOrders();
            var agentOrders = await FetchAgentOrders();

            if (invoices != null)
                model.Invoices = invoices.Data;
            if (payments != null)
            {
                model.Payments = payments.Data;
                PaymentStatistics(model.Stats, payments.Data.ResponseData);
            }

            if (orders != null)
            {
                model.Orders = orders.Data;
                OrderStatistics(model.Stats, orders.Data.ResponseData);
            }
            if (agentOrders != null)
            {
                model.AgentOrder = agentOrders.Data;
            }

            ViewBag.isDashboard = true;

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "AgentRolePolicy")]
        public async Task<IActionResult> EditLivestock(string id)
        {
            var token = HttpContext.Session.GetString("Token");
            var path = $"/api/v1/LiveStock/get-livestock/{id}";
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var response = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token, path);
            var content = response.Item1;
            var httpContent = response.Item2;
            if (httpContent.IsSuccessStatusCode)
            {
                var successResponse = JsonConvert.DeserializeObject<ResponseDto<LivestockResponseDto>>(content);
                var livestockFound = successResponse.Data;

                var resp = _mapper.Map<EditLivestockViewModel>(livestockFound);
                resp.MarketId = livestockFound.LivestockMarket.Id;
                resp.AgentId = livestockFound.LivestockAgent.Id;
                return View(resp);
            }

            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AgentRolePolicy")]
        public async Task<IActionResult> EditLivestock(EditLivestockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var livestockToUpdate = _mapper.Map<LivestockUpdateDto>(model);
                var token = HttpContext.Session.GetString("Token");
                var baseUrl = UrlHelper.BaseAddress(HttpContext);
                var response = await HttpHelper.PutContentAsync<LivestockUpdateDto>(baseUrl, livestockToUpdate, token, $"/api/v1/Livestock/update-livestock/{model.Id}");

                if (!response.Item2.IsSuccessStatusCode)
                {
                    var errResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(response.Item1);
                    foreach (var err in errResponse.Errs)
                    {
                        ModelState.AddModelError(err.Key, err.Value);
                    }
                    ViewBag.res = "notedited";
                    return View(model);
                }
                ViewBag.res = "edited";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult TrackOrders()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "AgentRolePolicy")]
        public async Task<IActionResult> AddLivestock(bool status, string opt)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var userid = HttpContext.Session.GetString("UserId");
            string agentId = "";

            ViewData["AddLivestockErrMsg"] = "false";

            if (status == true && opt == "AddLivestock")
            {
                ViewBag.LivesStockIsAdded = "true";
            }
            if (status == true && opt == "RestockLivestock")
            {
                ViewBag.Restocked = "true";
            }

            var marketResponse = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", "", $"/api/v1/Livestock/get-markets");
            if (marketResponse.Item2.IsSuccessStatusCode)
            {
                var marketSuccessResponse = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<AddLivestockMarketViewModel>>>(marketResponse.Item1);
                ViewData["Markets"] = marketSuccessResponse.Data;
            }
            var categoryResponse = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", "", $"/api/v1/Category/categories");
            if (categoryResponse.Item2.IsSuccessStatusCode)
            {
                var categorySuccessResponse = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<AddLivestockCategoryViewModel>>>(categoryResponse.Item1);
                ViewData["Categories"] = categorySuccessResponse.Data;
            }
            var agentIdResponse = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token, $"/api/v1/User/get-agentid/?userid={userid}");

            if (agentIdResponse.Item2.IsSuccessStatusCode)
            {
                var agentIdSuccessResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(agentIdResponse.Item1);
                agentId = agentIdSuccessResponse.Data;
                ViewData["AgentId"] = agentIdSuccessResponse.Data;
            }
            var agentLivestockResponse = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token, $"/api/v1/Livestock/get-agent-livestocks/?agentid={ agentId}");

            if (agentLivestockResponse.Item2.IsSuccessStatusCode)
            {
                var agentLivestockSuccessResponse = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<AgentLivestockViewModel>>>(agentLivestockResponse.Item1);

                ViewData["AgentLivestocks"] = agentLivestockSuccessResponse.Data;
            }

            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AgentRolePolicy")]
        public async Task<IActionResult> AddLivestock(AddLivestockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Session.GetString("Token");

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            if (model.Quantity > 0) model.Availability = true;
            var response = await HttpHelper.PostContentAsync(baseUrl, model, token, $"/api/v1/Livestock/add-livestock");

            if (response.Item2.IsSuccessStatusCode)
            {
                var addLivestockSuccessResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(response.Item1);

                await HttpHelper.UploadFileAsync(baseUrl, model.LivestockPhoto, token, $"/api/v1/Livestock/add-photo/{addLivestockSuccessResponse.Data}");

                return RedirectToAction("AddLivestock", new { opt = "AddLivestock", status = true });
            }

            var errorResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(response.Item1);

            foreach (var error in errorResponse.Errs)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            return RedirectToAction("AddLivestock");
        }

        [HttpPost]
        [Authorize(Policy = "AgentRolePolicy")]
        public async Task<IActionResult> RestockLivestock(RestockLivestockViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var responseObject = Utilities.CreateResponse("Invalid restock request", errs: ModelState, data: "");
                return BadRequest(responseObject);
            }

            var token = HttpContext.Session.GetString("Token");

            var baseUrl = UrlHelper.BaseAddress(HttpContext);

            if (model.Quantity > 0) model.Availability = true;

            var response = await HttpHelper.PostContentAsync(baseUrl, model, token, $"/api/v1/Livestock/restock-livestock");

            if (!response.Item2.IsSuccessStatusCode)
            {
                var errResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(response.Item1);
                foreach (var err in errResponse.Errs)
                {
                    ModelState.AddModelError(err.Key, err.Value);
                }

                return RedirectToAction("AddLivestock");
            }

            return RedirectToAction("AddLivestock", new { opt = "RestockLivestock", status = true });
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string q)
        {
            ViewBag.Q = q;
            var id = HttpContext.Session.GetString("UserId");
            var token = HttpContext.Session.GetString("Token");
            var path = $"/api/v1/User/get-user/{id}";
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (content, httpContent) = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token, path);
            if (httpContent.IsSuccessStatusCode)
            {
                var successResponse = JsonConvert.DeserializeObject<ResponseDto<UserToReturnDto>>(content);
                var profileFound = successResponse.Data;

                var res = _mapper.Map<UpdateProfileViewModel>(profileFound);
                res.Phone = profileFound.PhoneNumber;
                res.DeliveryAddress = new AddressViewModel();
                res.DeliveryAddress.Street = profileFound.Street;
                res.DeliveryAddress.City = profileFound.City;
                res.DeliveryAddress.State = (States)Enum.Parse(typeof(States), profileFound.State);

                var resp = new ProfileViewModel();
                resp.UpdateProfileViewModel = res;

                return View(resp);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile([FromQuery] string q, UpdateProfileViewModel model)
        {
            ViewBag.Q = "update-profile";
            var res = new ProfileViewModel();
            res.UpdateProfileViewModel = model;

            if (!ModelState.IsValid)
            {
                ViewBag.res = "notsaved";
                return View(res);
            }
            else
            {
                var id = HttpContext.Session.GetString("UserId");
                var userToUpdate = new UpdateUserDto()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    ZipCode = model.Zipcode,
                    Address = new Address()
                    {
                        UserId = id,
                        City = model.DeliveryAddress.City,
                        Street = model.DeliveryAddress.Street,
                        State = Enum.GetName(typeof(States), model.DeliveryAddress.State),
                    },
                };

                var token = HttpContext.Session.GetString("Token");
                var baseUrl = UrlHelper.BaseAddress(HttpContext);
                var response = await HttpHelper.PutContentAsync<UpdateUserDto>(baseUrl, userToUpdate, token, $"/api/v1/User/update-user/{id}");

                if (!response.Item2.IsSuccessStatusCode)
                {
                    var errResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(response.Item1);
                    foreach (var err in errResponse.Errs)
                    {
                        ModelState.AddModelError(err.Key, err.Value);
                    }
                    ViewBag.res = "notsaved";
                    return View(res);
                }
                ViewBag.res = "saved";
                return View("profile", res);
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PaymentHistory(int page, bool check)
        {
            var id = HttpContext.Session.GetString("UserId");
            var token = HttpContext.Session.GetString("Token");
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseBody, response) = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"api/v1/Order/payments-history-by-user/{id}?page={page}");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            response.EnsureSuccessStatusCode();
            var deserializedContent = JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<OrderDetailsViewModel>>>(responseBody);
            if (check == true)
            {
                return PartialView("PaymentHistoryPartialView", deserializedContent.Data);
            }

            return View(deserializedContent.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewBag.Q = "change-password";
            var resp = new ProfileViewModel();
            var update = _mapper.Map<ChangePasswordDto>(model);

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var userId = HttpContext.Session.GetString("UserId");
            var token = HttpContext.Session.GetString("Token");

            var (content, httpResponse) = await HttpHelper.PatchContentAsync(baseUrl, update, token, $"/api/v1/auth/change-password/{userId}");

            if (httpResponse.IsSuccessStatusCode)
            {
                ViewBag.resp = "saved";
                return View("profile", resp);
            }
            else
            {
                var errResponse = JsonConvert.DeserializeObject<ResponseDto<string>>(content);
                foreach (var error in errResponse.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                ViewBag.resp = "notsaved";
                return View("profile", resp);
            }
        }

        [HttpGet]
        [Authorize(Policy = "CustomerRolePolicy")]
        public async Task<IActionResult> Invoice([FromQuery] int page, [FromQuery] bool paginated)
        {
            string id = HttpContext.Session.GetString("UserId");
            string token = HttpContext.Session.GetString("Token");

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseBody, response) = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, "api/v1/Order/invoice/?" + $"page={page}&userid={id}");

            if (!response.IsSuccessStatusCode)
            {
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AccessDeniedHandler", "Error", new { StatusCode = 401 });
                }
            }

            var data = JsonConvert.DeserializeObject<ResponseDto<PagedInvoiceViewModel>>(responseBody);

            if (paginated)
            {
                return PartialView("InvoicePartial", data.Data);
            }
            return View(data.Data);
        }

        [HttpGet]
        [Authorize(Policy = "AdminAndDeliveryRolePolicy")]
        public async Task<IActionResult> DeliveryAssignment(bool check, int page)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            string token = HttpContext.Session.GetString("Token");

            var deseriliazed = await GetAssignmentsByStatus(token, page, UserId, "Pending");
            if (check)
                return PartialView("AssignmentPartialView", deseriliazed);
            return View(deseriliazed);
        }

        public async Task<IActionResult> AcceptedPartialView(int page)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            string token = HttpContext.Session.GetString("Token");
            var deserialized = await GetAssignmentsByStatus(token, page, UserId, "Accepted");
            return PartialView(deserialized);
        }

        public async Task<IActionResult> Accept(string assignmentId)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            string token = HttpContext.Session.GetString("Token");

            var baseUrl = UrlHelper.BaseAddress(HttpContext);

            var Response = await HttpHelper.PutContentAsync<AcceptDeliveryAssignmentViewModel>(
                                            baseUrl, null, token, $"/api/v1/Assignment/" +
                                            $"accept-assignment/{assignmentId}");

            var deserialized = await GetAssignmentsByStatus(token, 0, UserId, "Accepted");
            return PartialView("AcceptedPartialView", deserialized);
        }

        private async Task<ResponseDto<PaginatedResultDto
                                <AssignmentDeliveryViewModel>>> GetAssignmentsByStatus(string token, int page, string userId, string status)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var Response = await HttpHelper.GetContentAsync<string>(baseUrl, "", token,
                                            $"/api/v1/Assignment/get-assignments-by-user-and-status?" +
                                            $"page={page}&AppUserId={userId}&status={status}");

            var content = Response.Item1;
            var httpResponse = Response.Item2;
            httpResponse.EnsureSuccessStatusCode();

            var DeserializedContent = JsonConvert.DeserializeObject
                                    <ResponseDto<PaginatedResultDto<AssignmentDeliveryViewModel>>>(content);
            return DeserializedContent;
        }

        [HttpGet]
        [Authorize]
        [ActionName("Tracking-Orders")]
        public async Task<IActionResult> TrackOrders(string Id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/Tracking/get-tracking-by-num?number={Id}");
            var responsebody = result.Item1;
            var response = result.Item2;

            if (!response.IsSuccessStatusCode)
            {
                var errResponse = JsonConvert.DeserializeObject<ResponseDto<TrackingHistoryViewModel>>(responsebody);
                foreach (var err in errResponse.Errs)
                {
                    ModelState.AddModelError(err.Key, err.Value);
                }
                ViewData["Histories"] = null;
                return PartialView("TrackOrderPartial");
            }
            else
            {
                var successResponse = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<TrackingHistoryForUsersDto>>>(responsebody);
                var model = new TrackingHistoryViewModel();
                List<TrackingHistoryViewModel> toReturn = new List<TrackingHistoryViewModel>();
                foreach (var item in successResponse.Data.Reverse())
                {
                    var trackingHistory = new TrackingHistoryViewModel { Location = item.Location, OrderId = item.OrderId, Status = (TrackingHistoryStatus)item.Status, CustomerName = item.CustomerName, DeliveryPersonName = item.DeliveryPersonName, Description = item.Description };

                    toReturn.Add(trackingHistory);
                }
                ViewData["Histories"] = toReturn;
                return PartialView("TrackOrderPartial");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Orders(int page, bool check)
        {
            var token = HttpContext.Session.GetString("Token");
            // var userId = HttpContext.Session.GetString("UserId");
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseKey, response) = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token,
                                            $"/api/v1/Order/get-orders?page={page}");
            if (!response.IsSuccessStatusCode && (response.StatusCode != System.Net.HttpStatusCode.NotFound))
            {
                return RedirectToAction("Index");
            }

            var deserializedContent = JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<OrdersViewModel>>>(responseKey);

            //if (check == true)
            //    return PartialView("OrderPartialView", deserializedContent.Data);
            return View(deserializedContent.Data);
        }

        [HttpGet]
        public async Task<IActionResult> AgentOrders(int page)
        {

            if (HttpContext.Session.GetString("AgentPaid") != null)
            {
                ViewBag.AgentPaid = true;
                HttpContext.Session.Remove("AgentPaid");
            }
            if (HttpContext.Session.GetString("AgentPaidFailed") != null)
            {
                ViewBag.AgentPaidFailed = true;
                HttpContext.Session.Remove("AgentPaidFailed");
            }

            var token = HttpContext.Session.GetString("Token");
            // var userId = HttpContext.Session.GetString("UserId");
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseKey, response) = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token,
                                            $"/api/v1/Order/get-orderitems?page={page}");
            if (!response.IsSuccessStatusCode && (response.StatusCode != System.Net.HttpStatusCode.NotFound))
            {
                return RedirectToAction("Index");
            }

            // response.EnsureSuccessStatusCode();
            var deserializedContent = JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<ShapedListOfOrderItem>>>(responseKey);

            return View(deserializedContent.Data);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(string Id)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/Order/get-order/{Id}");
            var responsebody = result.Item1;
            var response = result.Item2;

            if (!response.IsSuccessStatusCode)
            {
                var errResponse = JsonConvert.DeserializeObject<ResponseDto<OrderToreturnByOrderIdDto>>(responsebody);
                return View(errResponse);
            }
            else
            {
                var successResponse = JsonConvert.DeserializeObject<ResponseDto<OrderToreturnByOrderIdDto>>(responsebody);

                var res = new OrdersDetailsViewModel
                {
                    TrackingNumber = successResponse.Data.TrackingNumber,
                    ShippedTo = successResponse.Data.ShippedTo,
                    DeliveryDate = successResponse.Data.DeliveryDate,
                    OrderItems = successResponse.Data.OrderItems,
                    Status = successResponse.Data.Status,
                    PaymentStatus = successResponse.Data.PaymentStatus
                };

                foreach (var item in successResponse.Data.OrderItems)
                {
                    res.PaymentAmount += item.Total;
                }
                ViewBag.Id = Id;

                return View(res);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder([FromForm] string Id, int page)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.PatchContentAsync(baseUrl, "", token, $"/api/v1/order/confirm-order/{Id}");

            var response = result.Item2;

            if (!response.IsSuccessStatusCode)
                return View(response);

            return RedirectToAction("Orders", "Dashboard", new { page = page });
        }

        [Authorize(Policy = "AdminAndAgentRolePolicy")]
        public async Task<IActionResult> Livestock(bool check, int page)
        {
            var token = HttpContext.Session.GetString("Token");
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var Response = await HttpHelper.GetContentAsync<string>(baseUrl, "", token,
                                            $"/api/v1/LiveStock/get-livestocks?page={page}");

            var content = Response.Item1;
            var httpResponse = Response.Item2;
            httpResponse.EnsureSuccessStatusCode();

            var DeserializedContent = JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<ShappedListOfLivestock>>>(content);
            if (check)
                return PartialView("LivestockPartialView", DeserializedContent.Data);
            return View(DeserializedContent.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAgentPayment(string paymentType)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var id = HttpContext.Session.GetString("UserId");


            if (_config["AgentPayment:AgentFee"] == null)
            {
                HttpContext.Session.SetString("Error", "Payment was not successful");
                return RedirectToAction("Profile", new { q = "update-profile" }); // return error view 
            }

            var response = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"api/v1/User/get-user/{id}");

            var content = response.Item1;
            var httpResponse = response.Item2;

            if (!httpResponse.IsSuccessStatusCode)
            {
                return View(); //Return error in viewbag
            }

            var DeserializedContent = JsonConvert.DeserializeObject<ResponseDto<UserViewModel>>(content);
            var data = DeserializedContent.Data;
            if (paymentType == "paystack")
            {
                var request = new PaystackRequestDto
                {
                    amount = _config["AgentPayment:AgentFee"],
                    email = data.Email,
                    callback_url = UrlHelper.BaseAddress(HttpContext) + "/Dashboard/activateagent"
                };
                var result = await PaystackPayment.InitiatePayment(request, _config["Paystack:BaseUrl"], "initialize", _config["Paystack:Token"]);

                return Redirect(result.Data.AuthorizationUrl);
            }
            else
            {
                HttpContext.Session.SetString("flutterpay", JsonConvert.SerializeObject(
                    new FlutterAgentPayDto
                    {
                        Amount = decimal.Parse(_config["AgentPayment:AgentFee"]),
                        Email = data.Email
                    }));

                return RedirectToAction("Initiate", "Checkout");
            }
        }

        [HttpGet]
        [Authorize(Policy = "DeliveryRolePolicy")]
        public async Task<IActionResult> AddTracking(bool status, int page = 1)
        {
            if (status == true)
            {
                ViewBag.TrackingAdded = "true";
            }

            string UserId = HttpContext.Session.GetString("UserId");
            string token = HttpContext.Session.GetString("Token");
            var deserialized = await GetAssignmentsByStatus(token, page, UserId, "Accepted");
            // get orders accepted by delivery person
            ViewBag.OrderToTrack = deserialized.Data;
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "DeliveryRolePolicy")]
        public async Task<IActionResult> AddTracking(AddTrackingHistoryViewModel model)

        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Session.GetString("Token");

            var userId = HttpContext.Session.GetString("UserId");

            var baseUrl = UrlHelper.BaseAddress(HttpContext);

            model.UserId = userId;
            var trackingModel = _mapper.Map<AddTrackingWithStatesStringViewModel>(model);
            var response = await HttpHelper.PostContentAsync(baseUrl, trackingModel, token, $"/api/v1/Tracking/add-tracking");

            if (!response.Item2.IsSuccessStatusCode)
            {
                return RedirectToAction("AddTracking", new { status = false });
            }

            return RedirectToAction("AddTracking", new { status = true });
        }

        [HttpGet]
        public async Task<IActionResult> ActivateAgent()
        {
            var verify = true;

            if (HttpContext.Session.GetString("verifyDetails") != null)
            {
                var verifyResponse = JsonConvert.DeserializeObject<FlutterwaveResponseDto>(HttpContext.Session.GetString("verifyDetails"));
                verify = await FlutterwavePayment.VerifyPayment(verifyResponse.data.id, verifyResponse.data.amount, _config["Flutterwave:BaseUrl"], _config["Flutterwave:SecretKey"]);
            }

            if (!verify)
                return View();//Pass error message in viewbag

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var id = HttpContext.Session.GetString("UserId");

            var activationResponse = await HttpHelper.PatchContentAsync(baseUrl, "", token, $"/api/v1/Auth/activate-account/{id}");

            if (!activationResponse.Item2.IsSuccessStatusCode)
            {
                return View("Profile");//Pass error message in viewbag
            }

            HttpContext.Session.SetString("activeStatus", "True");
            return RedirectToAction("Index", "Dashboard", new { status = "Activated" });
        }

        public async Task<IActionResult> AgentOrderItemDetails(string id, string page)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/LiveStock/get-livestock/{id}");
            var responsebody = result.Item1;
            var response = result.Item2;

            if (!response.IsSuccessStatusCode)
            {
                var errResponse = JsonConvert.DeserializeObject<ResponseDto<LivestockResponseDto>>(responsebody);
                return View(errResponse);
            }
            else
            {
                var successResponse = JsonConvert.DeserializeObject<ResponseDto<LivestockResponseDto>>(responsebody).Data;

                var res = new DisplayOrderItemAgent
                {
                    Age = successResponse.Age,
                    Breed = successResponse.Breed,
                    SellingPrice = successResponse.SellingPrice,
                    PurchasePrice = successResponse.PurchasePrice,
                    Sex = successResponse.Sex,
                    Weight = successResponse.Weight,
                    Description = successResponse.Description,
                    Color = successResponse.Color
                };

                ViewBag.page = page;

                return View(res);
            }


        }
        public async Task<IActionResult> AgentPaymentRequest(string orderId, string itemId)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.PatchContentAsync(baseUrl, "", token, $"/api/v1/Order/request-payment/{orderId}/{itemId}");
            var response = result.Item2;

            if (!response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("AgentPaidFailed", "AgentPaidFailed");
                return RedirectToAction("AgentOrders", "Dashboard");
            }


            HttpContext.Session.SetString("AgentPaid", "AgentPaid");

            return RedirectToAction("AgentOrders", "Dashboard");
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeliveryPersonRegistration()
        {
            if (HttpContext.Session.GetString("agentAdded") != null)
            {
                ViewData["agentAdded"] = true;
                HttpContext.Session.Remove("agentAdded");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeliveryPersonRegistration(AddDeliveryPersonViewModel model)
        {
            // set the userName to Email
            model.UserName = model.Email;
            var deliveryPerson = _mapper.Map<RegisterDeliveryPersonDto>(model);
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var response = await HttpHelper.PostContentAsync<RegisterDeliveryPersonDto>(baseUrl, deliveryPerson, "/api/v1/user/add-delivery-person");
            var content = response.Item1;
            var httpResponse = response.Item2;
            if (httpResponse.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("agentAdded", "true");
                return RedirectToAction("DeliveryPersonRegistration");
            }
            else
            {
                var deserializedContentErrors = JsonConvert.DeserializeObject<ResponseDto<string>>(content);
                foreach (var error in deserializedContentErrors.Errs)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return View(model);
            }
        }

        public async Task<IActionResult> Logs()
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var response = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, "api/v1/logs");
            if (response.Item2.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<string[]>(response.Item1);
                List<string> dataToReturn = new List<string>();
                foreach (var d in data)
                {
                    dataToReturn.Add(
                            d.Split(@"\").LastOrDefault()
                        );
                }
                return View(dataToReturn);
            }
            else
            {
                ViewData["fetchError"] = true;
                return View(new List<string>());
            }
        }

        //[HttpGet("{fileId}")]
        public async Task<IActionResult> Log([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return RedirectToAction("Logs");
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var response = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, "api/v1/logs/" + id);
            if (response.Item2.IsSuccessStatusCode)
            {
                var fetchedData = JsonConvert.DeserializeObject<ResponseDto<string>>(response.Item1);
                var data = fetchedData.Data.Split("\r\n");
                ViewData["file"] = id;
                return View(data);
            }

            return RedirectToAction("Logs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLog(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) RedirectToAction("Logs");
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var response = await HttpHelper.PostContentAsync<string>(baseUrl, id, token, "api/v1/logs/" + id);

            return RedirectToAction("Logs");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/User/get-user/{id}");
            var responsebody = result.Item1;
            var response = result.Item2;
            if (!response.IsSuccessStatusCode)
            {
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AccessDeniedHandler", "Error", new { StatusCode = 401 });
                }
            }
            var successResponse = JsonConvert.DeserializeObject<ResponseDto<SingleUserVm>>(responsebody);
            return Json(successResponse.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(string id)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/User/get-user/{id}");
            var responsebody = result.Item1;
            var response = result.Item2;
            if (!response.IsSuccessStatusCode)
            {
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AccessDeniedHandler", "Error", new { StatusCode = 401 });
                }
            }
            var successResponse = JsonConvert.DeserializeObject<ResponseDto<SingleUserVm>>(responsebody);
            return Json(successResponse.Data);
        }

        private Tuple<string, string> AuthUserInfo()
        {
            var id = HttpContext.Session.GetString("UserId");
            var token = HttpContext.Session.GetString("Token");

            return Tuple.Create(id, token);
        }

        private async Task<ResponseDto<PagedInvoiceViewModel>> FetchUserInvoices(int page = 1)
        {
            var (id, token) = AuthUserInfo();

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseBody, response) = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, "api/v1/Order/invoice/?" + $"page={page}&userid={id}");

            return response.IsSuccessStatusCode ?
                JsonConvert.DeserializeObject<ResponseDto<PagedInvoiceViewModel>>(responseBody)
                : null;
        }

        private async Task<ResponseDto<PaginatedResultDto<ShapedListOfOrderItem>>> FetchAgentOrders(int page = 1)
        {
            var (id, token) = AuthUserInfo();
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseBody, response) = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token,
                                            $"/api/v1/Order/get-orderitems?page={page}");

            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<ShapedListOfOrderItem>>>(responseBody)
                : null;
        }

        private async Task<ResponseDto<PaginatedResultDto<OrderDetailsViewModel>>> FetchUserPaymentHistory(int page = 1)
        {
            var (id, token) = AuthUserInfo();

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseBody, response) = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"api/v1/Order/payments-history-by-user/{id}?page={page}");

            return response.IsSuccessStatusCode ?
                JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<OrderDetailsViewModel>>>(responseBody)
                : null;
        }

        private async Task<ResponseDto<PaginatedResultDto<OrdersViewModel>>> FetchUserOrders(int page = 1)
        {
            var (_, token) = AuthUserInfo();

            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var (responseKey, response) = await HttpHelper.GetContentWithTokenAsync<string>(baseUrl, "", token,
                $"/api/v1/Order/get-orders?page={page}");

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<OrdersViewModel>>>(responseKey)
                : null;
        }

        private static void PaymentStatistics(StatsViewModel stats, IEnumerable<OrderDetailsViewModel> paymentData)
        {
            var uniqueDateTime = paymentData.Select(payment => payment.DateCreated).Distinct().ToArray();

            var dates = new string[uniqueDateTime.Length];
            for (var i = 0; i < uniqueDateTime.Length; i++)
            {
                dates[i] = uniqueDateTime[i].Split(" ")[0];
            }
            dates = dates.Distinct().ToArray();

            var length = dates.Count();
            var completePaymentsStats = new int[length];
            var incompletePaymentsStats = new int[length];

            var count = 0;
            foreach (var date in dates)
            {
                var particularDayPayment = paymentData.Where(payment => payment.DateCreated.Split(" ")[0] == date);

                foreach (var payment in particularDayPayment)
                {
                    if (payment.Status == 1)
                        completePaymentsStats[count]++;
                    else
                        incompletePaymentsStats[count]++;
                }
                count++;
            }
            stats.PaymentDates = dates;
            stats.CompletePayments = completePaymentsStats;
            stats.IncompletePayments = incompletePaymentsStats;
        }

        private static void OrderStatistics(StatsViewModel stats, IEnumerable<OrdersViewModel> orderData)
        {
            var uniqueDateTime = orderData.Select(order => order.DateCreated).Distinct().ToArray();

            var dates = new string[uniqueDateTime.Length];
            for (var i = 0; i < uniqueDateTime.Length; i++)
            {
                dates[i] = uniqueDateTime[i].Split(" ")[0];
            }

            dates = dates.Distinct().ToArray();

            var length = dates.Count();
            var numOfOrders = new int[length];

            var count = 0;
            foreach (var date in dates)
                numOfOrders[count++] = orderData.Count(order => order.DateCreated.Split(" ")[0] == date);

            stats.OrderDates = dates;
            stats.NumOfOrders = numOfOrders;
        }

        [HttpGet]
        public async Task<IActionResult> RegisteredUsers(int page)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/User/get-all-users/?page=" + page);
            var responsebody = result.Item1;
            var response = result.Item2;
            if (!response.IsSuccessStatusCode)
            {
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AccessDeniedHandler", "Error", new { StatusCode = 401 });
                }
            }

            var successResponse = JsonConvert.DeserializeObject<ResponseDto<PaginatedResultDto<UserVm>>>(responsebody);

            return View(successResponse.Data);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAgentsMarket([FromQuery] string id)
        {
            var baseUrl = UrlHelper.BaseAddress(HttpContext);
            var token = HttpContext.Session.GetString("Token");
            var result = await HttpHelper.GetContentWithTokenAsync(baseUrl, "", token, $"/api/v1/LiveStock/get-agent-livestocks/?agentid={id}");
            var responsebody = result.Item1;
            var response = result.Item2;
            if (!response.IsSuccessStatusCode)
            {
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AccessDeniedHandler", "Error", new { StatusCode = 401 });
                }
            }

            var successResponse = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<LivestockMarketVm>>>(responsebody);

            return View(successResponse.Data);
        }
    }
}
