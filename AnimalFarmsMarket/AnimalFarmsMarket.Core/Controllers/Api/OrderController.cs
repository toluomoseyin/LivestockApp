using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AnimalFarmsMarket.Data.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly int _Page;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IOrderItemsService _orderitems;
        private readonly IOrderService _orderService;
        

              
        public OrderController(IOrderService orderService, IConfiguration configuration, IMapper mapper, IOrderItemsService orderItemsService, UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _mapper = mapper;
            _orderitems = orderItemsService;
            _Page = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("get-orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int page)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);
            //var _perPage = 9;
            page = page <= 0 ? 1 : page;

            IEnumerable<Order> result = null;

            foreach (var role in roles)
            {
                if (role == "Admin")
                {
                    result =  await _orderService.GetOrders(page, _Page);
                    break;
                }
                if (role == "Customer")
                {
                    result = await _orderService.GetOrderByUserIdAsync(currentUser.Id, page, _Page);
                    break;
                }
            }

            if (result == null || result.Count() <1 )
            {
                ModelState.AddModelError("Not found", $"No result found page {page}");
                return NotFound(Utilities.CreateResponse(message: "Order not found", errs: ModelState, ""));
            }

            var orderentity = _mapper.Map<IEnumerable<ListOfOrderDto>>(result);

            var pageMetaData = Utilities.Paginate(page, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<ListOfOrderDto> { PageMetaData = pageMetaData, ResponseData = orderentity };
            return Ok(Utilities.CreateResponse(message: "List of orders paginated by 10", errs: null, data: pagedItems));
        }


        [Authorize(Roles = "Agent")]
        [HttpGet("get-orderitems")]
        public async Task<IActionResult> GetAllOrderItemsByAgentId([FromQuery] int page)
        {

            var currentUser = await _userManager.GetUserAsync(User);

            page = page <= 0 ? 1 : page;

            IEnumerable<ShapedListOfOrderItem> result = null;

            result = await _orderService.GetOrderByAgentIdAsync(currentUser.Id, page, _Page);

            if (result == null || result.Count() < 1)
            {
                ModelState.AddModelError("Not found", $"No result found page {page}");
                return NotFound(Utilities.CreateResponse(message: "Order Items not found", errs: ModelState, ""));
            }

            //var orderItemEntity = _mapper.Map<IEnumerable<ListOfOrderDto>>(result);

            var pageMetaData = Utilities.Paginate(page, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<ShapedListOfOrderItem> { PageMetaData = pageMetaData, ResponseData = result };
            return Ok(Utilities.CreateResponse(message: "List of orders paginated by 10", errs: null, data: pagedItems));
        }



        [Authorize]
        [HttpGet("get-order/{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("InvalidId", "OrderId must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, ""));
            }

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                ModelState.AddModelError("Not found", "OrderId does not exist");
                var resp = Utilities.CreateResponse(message: "Invalid OrderId", errs: ModelState, data: "");
                return NotFound(resp);
            }

            var response = _mapper.Map<OrderToreturnByOrderIdDto>(order);
            return Ok(Utilities.CreateResponse("Order Details", null, response));

        }

        [Authorize]
        [HttpGet("get-order-by-trackingid/{id}")]
        public async Task<IActionResult> GetOrderByTrackingId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Invalid-trackinId", "TrackingId must be provided");
                return BadRequest(Utilities.CreateResponse("trackingId was not provided", ModelState, ""));
            }

            var order = await _orderService.GetOrderByTrackingIdAsync(id);

            if (order == null)
            {
                ModelState.AddModelError("Not found", "TrackingId does not exist");
                var resp = Utilities.CreateResponse(message: "Invalid TrackingId", errs: ModelState, data: "");
                return NotFound(resp);
            }

            var response = _mapper.Map<OrderToreturnByTrackingIdDto>(order);
            return Ok(Utilities.CreateResponse("Order Details", null, response));

        }

        [Authorize]
        [HttpGet("get-orders-by-status")]
        public async Task<IActionResult> GetOrderByConfirmationStatus([FromQuery] int page, [FromQuery] byte confirmationstatus)
        {

            page = page <= 0 ? 1 : page;
            var order = await _orderService.GetOrderByConfirmationStatusAsync(confirmationstatus, page, _Page);

            if (order == null || (order.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No result found page {page}");
                return NotFound(Utilities.CreateResponse(message: "order not found", errs: ModelState, ""));
            }

            var OrderRes = _mapper.Map<IEnumerable<OrdersbyStatusDto>>(order);

            var pageMetaData = Utilities.Paginate(page, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<OrdersbyStatusDto> { PageMetaData = pageMetaData, ResponseData = OrderRes };

            return Ok(Utilities.CreateResponse("Order gotten by status", null, pagedItems));

        }


        [Authorize(Roles="Customer")]
        [HttpGet("invoice")]
        public async Task<IActionResult> GetUserOrder([FromQuery] InvoiceQuery invoiceQuery)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            if(activeUser.Id != invoiceQuery.userid)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }
            if (!activeUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            var page = invoiceQuery.page <= 0 ? 1 : invoiceQuery.page;
            var orders = _orderitems.GetUserOrders(invoiceQuery.userid, invoiceQuery.page, _Page);

           var res = new List<OrderToreturnByOrderIdDto>();
           foreach (var item in orders)
           {
               
               var res2 = new OrderToreturnByOrderIdDto();
               res2.Customer = _mapper.Map<OrderUserResDto>(item.ToList()[0].Order.User);
               res2.Id = item.Key;
               res2.Status = item.ToList()[0].Order.Status;
               res2.TrackingNumber = item.ToList()[0].Order.TrackingNumber;
               res2.DeliveryMode = _mapper.Map<DeliveryModeDto>(item.ToList()[0].Order.DeliveryMode);
               res2.ShippingPlan = _mapper.Map<ShippingDto>(item.ToList()[0].Order.ShippingPlan);
               res2.PaymentAmount = item.ToList()[0].Order.PaymentAmount;
               res2.PaymentMethod = _mapper.Map<PaymentMethodDto>(item.ToList()[0].Order.PaymentMethod);
               res2.PaymentStatus = item.ToList()[0].Order.PaymentStatus;
               res2.ShippedTo = item.ToList()[0].Order.ShippedTo;
               res2.DeliveryDate = item.ToList()[0].Order.DeliveryDate;
               res2.OrderItems = _mapper.Map<ICollection<OrderItemResDto>>(item.ToList());
               res.Add(res2);
           };

            var pageMetaData = Utilities.Paginate(page, _Page, _orderitems.GetCount());
            var pagedItems = new PaginatedResultDto<OrderToreturnByOrderIdDto> { PageMetaData = pageMetaData, ResponseData = res };
            var response = pagedItems;
            return Ok(Utilities.CreateResponse("Order gotten by loggedInUser", null, response));
        }

        [Authorize(Roles = "Customer")]
        [Route("add-order")]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderForCreationDto orderToAdd)
        {
            if (!ModelState.IsValid)
            {
                var responseObject = Utilities.CreateResponse("Invalid order request", errs: ModelState, data: "");
                return BadRequest(responseObject);
            }

            var orderEntity = _mapper.Map<Order>(orderToAdd);
            var extendedPeriod = Convert.ToDouble(orderToAdd.DeliveryPeriod);
            orderEntity.DeliveryDate = (DateTime.Now.AddDays(extendedPeriod).ToString());
            orderEntity.TrackingNumber = TrackingNumberGen.RandomString();

            orderEntity.DeliveryAssignment = new DeliveryAssignment
                                            {
                                                DateCreated = DateTime.Now.ToString(),
                                                DateUpdated = DateTime.Now.ToString()
                                            };

            if (!await _orderService.AddOrderAsync(orderEntity))
            {
                ModelState.AddModelError("Error", "There was an error adding order");
                var responseOb = Utilities.CreateResponse("Could not add order successfully", ModelState, "");
                return BadRequest(responseOb);
            }

            var response = Utilities.CreateResponse("Order added successfully!", null, "");

            return Created("Order successfully", response);
        }

        [HttpPost("get-temp-order")]
        public async Task<IActionResult> TempOrder([FromBody] OrderForCreationDto orderToAdd)
        {
            if (!ModelState.IsValid)
            {
                var responseObject = Utilities.CreateResponse("Invalid order request", errs: ModelState, data: "");
                return BadRequest(responseObject);
            }

            var orderEntity = _mapper.Map<Order>(orderToAdd);
            var extendedPeriod = Convert.ToDouble(orderToAdd.DeliveryPeriod);
            orderEntity.DeliveryDate = (DateTime.Now.AddDays(extendedPeriod).ToString());
            orderEntity.TrackingNumber = TrackingNumberGen.RandomString();
            var order = await _orderService.CalculateOrderPrice(orderEntity);

            return Ok(Utilities.CreateResponse<Order>("Order Created", null,  order));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-order/{orderid}")]
        public async Task<IActionResult> DeleteOrder(string orderid)
        {
            if (string.IsNullOrWhiteSpace(orderid))
            {
                ModelState.AddModelError("Error", "Id is not provided");
                return BadRequest(Utilities.CreateResponse("Id must be provided", ModelState, ""));
            }

            var orderToDelete = await _orderService.GetOrderByIdAsync(orderid);

            if (orderToDelete == null)
            {
                ModelState.AddModelError("Id", "Id does not exist");
                return NotFound(Utilities.CreateResponse("No record found", ModelState, ""));
            }

            var orderResponse = await _orderService.DeleteOrderAsync(orderToDelete);
            if (!orderResponse)
            {
                ModelState.AddModelError("Order", "Could not delete order");
                return BadRequest(Utilities.CreateResponse("Error", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse("Order deleted sucessfully", errs: null, data: ""));
        }


        [Authorize]
        [HttpGet("get-orders-by-payment")]
        public async Task<IActionResult> GetOrderByPaymentStatus([FromQuery] int PageNum, byte PaymentStatus)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Not Found", $"no result found for payment status {PaymentStatus} ");
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var order = await _orderService.GetOrdersByPaymentStatusAsync(PaymentStatus, PageNum, _Page);
            var orderToReturn = _mapper.Map<IEnumerable<OrdersbyStatusDto>>(order);

            var pageMetaData = Utilities.Paginate(PageNum, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<OrdersbyStatusDto> { PageMetaData = pageMetaData, ResponseData = orderToReturn };
            return Ok(Utilities.CreateResponse(message: "List of Orders", errs: null, data: pagedItems));
        }

        [Authorize]
        [HttpGet("Get-Order-By-Livestock")]
        public async Task<IActionResult> GetOrderByLiveStock([FromQuery] string LiveStockId, int PageNum)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Not Found", $"no result found for live stock id {LiveStockId} ");
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var order = await _orderService.GetOrderByLiveStockAsync(PageNum, _Page, LiveStockId);


            var orderToReturn = _mapper.Map<IEnumerable<OrdersbyStatusDto>>(order);


            var pageMetaData = Utilities.Paginate(PageNum, _Page, _orderService.TotalCount);

            var pagedItems = new PaginatedResultDto<OrdersbyStatusDto> { PageMetaData = pageMetaData, ResponseData = orderToReturn };
            return Ok(pagedItems);
        }

        //get orders by status --Onas end point
        [Authorize]
        [HttpGet("get-order-by-stat")]
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] byte status, [FromQuery] int PageNum)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Not Found", $"no result found for status {status} ");
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var order = await _orderService.GetOrdersByStatusAsync(status, PageNum, _Page);
            var orderToReturn = _mapper.Map<IEnumerable<OrdersbyStatusDto>>(order);

            var pageMetaData = Utilities.Paginate(PageNum, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<OrdersbyStatusDto> { PageMetaData = pageMetaData, ResponseData = orderToReturn };
            return Ok(pagedItems);
        }

        [Authorize]
        [HttpGet("payments-history-by-user/{id}")]
        public async Task<IActionResult> GetCustomerPaymentHistory(string id, [FromQuery] int page)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Id", "No id inputted");
                return BadRequest(Utilities.CreateResponse("No id found", ModelState, ""));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("NotFound", "User not Found");
                return NotFound(Utilities.CreateResponse("User Not Found", ModelState, ""));
            }
            var role = await _userManager.GetRolesAsync(currentUser);
            page = page <= 0 ? 1 : page;
            if (role.Contains("Admin"))
            {
                var adminRes = await _orderService.GetOrdersByAdminAsync(page, _Page);
                if (adminRes == null || adminRes.Count() < 0)
                {
                    ModelState.AddModelError("Not found", $"No result found page {page}");
                    return NotFound(Utilities.CreateResponse(message: "Payments not found", errs: ModelState, ""));
                }
                var resultData = _mapper.Map<IEnumerable<PaymentHistoryDto>>(adminRes);
                var pagedMetaData = Utilities.Paginate(page, _Page, _orderService.TotalCount);
                var pagedItemsAdmin = new PaginatedResultDto<PaymentHistoryDto> { PageMetaData = pagedMetaData, ResponseData = resultData };

                return Ok(Utilities.CreateResponse(message: "List of Payments History paginated by 10", errs: null, data: pagedItemsAdmin));
            }

            if (currentUser.Id != id)
            {
                ModelState.AddModelError("Id", "Do not have authorization access");
                return Unauthorized(Utilities.CreateResponse("Unauthorized", ModelState, ""));
            }


            var res = await _orderService.GetOrdersByCustomerIdAsync(id, page, _Page);
            if (res == null || res.Count() < 0)
            {
                ModelState.AddModelError("Not found", $"No result found page {page}");
                return NotFound(Utilities.CreateResponse(message: "Payments not found", errs: ModelState, ""));
            }

            var result = _mapper.Map<IEnumerable<PaymentHistoryDto>>(res);

            var pageMetaData = Utilities.Paginate(page, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<PaymentHistoryDto> { PageMetaData = pageMetaData, ResponseData = result };

            return Ok(Utilities.CreateResponse(message: "List of Payments History paginated by 10", errs: null, data: pagedItems));

        }

        [Authorize]
        [HttpGet("get-customer-orders")]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] int page, string userid)
        {
            page = page <= 0 ? 1 : page;

            var orders = await _orderService.GetOrdersByLoggedInUserAsync(userid, page, _Page );


            if (orders == null || (orders.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No result found page {page}");
                return NotFound(Utilities.CreateResponse(message: "Orders not found", errs: ModelState, ""));
            }

            List<GetOrderDto> ordersToReturn = new List<GetOrderDto>();

            foreach (var item in orders)
            {
                foreach (var orderItem in item.OrderItems)
                {
                    ordersToReturn.Add(new GetOrderDto
                    {
                        Id = item.Id,
                        Breed = orderItem.Livestock.Breed,
                        Availability = orderItem.Livestock.Availability,
                        CreatedAt = orderItem.DateUpdated,
                        Status = item.Status
                    });
                }
            }

            var pageMetaData = Utilities.Paginate(page, _Page, _orderService.TotalCount);
            var pagedItems = new PaginatedResultDto<GetOrderDto> { PageMetaData = pageMetaData, ResponseData = ordersToReturn };
            return Ok(pagedItems);

        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpPatch("confirm-order/{OrderId}")]
        public async Task<IActionResult> OrderDetails(string OrderId)
        {
            var order = await _orderService.FindOrderByIdAsync(OrderId);

            if (order == null)
            {
                ModelState.AddModelError("Not found", $"No Order founf for the ID: {OrderId}");
                return NotFound(Utilities.CreateResponse(message: "Orders not found", errs: ModelState, ""));
            }

            order.Status = 1;
            order.PaymentStatus = 1;

            var orderResponse = await _orderService.UpdateOrderByIdAsync(order);

            if (!orderResponse)
            {
                ModelState.AddModelError("Order", "Could not confirm order status");
                return BadRequest(Utilities.CreateResponse("Error", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse("Order status confirmed sucessfully", errs: null, data: ""));
        }

        [Authorize(Roles = "Agent")]
        [HttpPatch("request-payment/{orderId}/{itemId}")]
        public async Task<IActionResult> AgentRequestPayment(string orderId, string itemId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                ModelState.AddModelError("Not found", $"No Order found for the ID: {orderId}");
                return NotFound(Utilities.CreateResponse(message: "Order not found", errs: ModelState, ""));
            }

            order.OrderItems.Where(x => x.Id == itemId).SingleOrDefault().AgentPaid = 1;

            var orderResponse = await _orderService.UpdateOrderByIdAsync(order);

            if (!orderResponse)
            {
                ModelState.AddModelError("Order Item", "Could not Request Payment");
                return BadRequest(Utilities.CreateResponse("Error", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse("Payment made successfully", errs: null, data: ""));
        }
    }
}
