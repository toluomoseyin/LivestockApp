using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Enum;
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
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssignmentController : ControllerBase
    {
        private readonly IDeliveryAssignmentService _deliveryAssignmentService;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int _perPage;
        private const int resCountLimit = 1;

        public AssignmentController(IDeliveryAssignmentService deliveryAssignmentService, IConfiguration configuration, IMapper mapper, IOrderService orderService, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _deliveryAssignmentService = deliveryAssignmentService;
            _mapper = mapper;
            _orderService = orderService;
            _userManager = userManager;
            _roleManager = roleManager;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
        }



        [Authorize(Roles ="Delivery")]
        [HttpGet("get-assignments-by-status")]
        public async Task<IActionResult> GetAssignmentByStatus([FromQuery] int page,[FromQuery]string status)
        {

            if (string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("Assignment", "An empty Delivery assignment status was passed");
                return BadRequest(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "An empty Delivery assignment status was passed", errs: ModelState, null));
            }

            page = page <= 0 ? 1 : page;

            // checks if the incoming string matches any value in the Delivery Assignment status enum
            if (!Enum.IsDefined(typeof(DeliveryAssignmentStatus),Utilities.ChangeToTitleCase(status)))
            {
                ModelState.AddModelError("Assignment", "Not a valid Delivery Assignment status");
                return BadRequest(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "No assignment was found", errs: ModelState, null));
            }

            // Converts the string to Enum a Delivery assignment enum
            var enumStatus = (DeliveryAssignmentStatus)Enum
                        .Parse(typeof(DeliveryAssignmentStatus), status, true);

            var assignments = await _deliveryAssignmentService.GetAssignmentsByStatusAsync(page,_perPage,(byte)enumStatus);

            if (assignments.Count() < resCountLimit)
            {
                ModelState.AddModelError("assignment", "No delivery assignment was found");
                return Ok(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "No assignment was found", errs: ModelState, null));
            }

            var pagedItems = AddPaginationToResult(assignments, page);  
            return Ok(Utilities.CreateResponse(message: "All delivery assignment by status", errs: null, data: pagedItems));
           
         }




       [Authorize(Roles = "Delivery")]
        [HttpGet("get-assignments-by-order")]
        public async Task<IActionResult> GetAssignmentByOrder([FromQuery] int page,[FromQuery] string orderId)
        {

            if (string.IsNullOrEmpty(orderId))
            {
                ModelState.AddModelError("Assignment", "An empty Order Id was passed");
                return BadRequest(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "An empty Order Id was passed", errs: ModelState, null));
            }

            page = page <= 0 ? 1 : page;

            var assignments = await _deliveryAssignmentService.GetAssignmentsByOrderAsync(page, _perPage, orderId);

            if (assignments.Count() < resCountLimit)
            {
                ModelState.AddModelError("assignment", "No delivery assignment was found");
                return Ok(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "No delivery assignment was found", errs: ModelState, null));
            }

            var pagedItems = AddPaginationToResult(assignments, page);

            return Ok(Utilities.CreateResponse(message: "All delivery assignment by order", errs: null, data: pagedItems));
        }




       [Authorize(Roles = "Delivery")]
        [HttpGet("get-assignments-by-delivery-person")]
        public async Task<IActionResult> GetAssignmentByDeliveryPerson([FromQuery] int page, [FromQuery]string personId)
        {

            if (string.IsNullOrEmpty(personId))
            {
                ModelState.AddModelError("Assignment", "An empty Delivery Person Id was passed");
                return BadRequest(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "An empty Delivery Person Id was passed", errs: ModelState, null));
            }

            page = page <= 0 ? 1 : page;

            var assignments = await _deliveryAssignmentService.GetAssignmentsByDeliveryPersonAsync(page, _perPage, personId);

            if (assignments.Count() < resCountLimit)
            {
                ModelState.AddModelError("assignments", "No delivery assignment was found");
                return Ok(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>(message: "No delivery assignment was found", errs: ModelState, null));
            }

            var pagedItems = AddPaginationToResult(assignments,page);

            return Ok(Utilities.CreateResponse(message: "All delivery assignment by delivery person", errs: null, data: pagedItems));
        }




        private PaginatedResultDto<ShapedListOfDeliveryAssignment> AddPaginationToResult(IEnumerable<ShapedListOfDeliveryAssignment> assignments, int page )
        {
            var pageMetaData = Utilities.Paginate(page, _perPage, _deliveryAssignmentService.GetTotalCount());
            var pagedItems = new PaginatedResultDto<ShapedListOfDeliveryAssignment> { PageMetaData = pageMetaData, ResponseData = assignments };
            return pagedItems;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get-assignments")]
        public async Task<IActionResult> GetAssignments([FromQuery] int pageNumber)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var result = await _deliveryAssignmentService.DeliveryAssignments(pageNumber, _perPage);
            if (result.Count() < resCountLimit)
            {
                ModelState.AddModelError("Assignment", $"Assignment not found");
                return NotFound(Utilities.CreateResponse("Assignment not found", ModelState, ""));
            }
            var pagedItems = AddPaginationToResult(result, pageNumber);
            return Ok(Utilities.CreateResponse("List of Assignments", null, pagedItems));
        }


        [Authorize(Roles = "Admin, Delivery")]
        [HttpGet("get-assignment/{id}")]
        public async Task<IActionResult> GetAssignment(string id)
        {
            var result = await _deliveryAssignmentService.GetAssignmentByIdAsync(id);
            if (result == null)
            {
                ModelState.AddModelError("Assignment", "Assignment does not exist");
                return NotFound(Utilities.CreateResponse("Assignment not found", ModelState, ""));
            }
            var resp = _mapper.Map<ShapedListOfDeliveryAssignment>(result);
            resp.Status = ((DeliveryAssignmentStatus)result.Status).ToString();
            return Ok(Utilities.CreateResponse("Assignment Details", null, resp));
        }


        [Authorize(Roles = "Admin, Delivery")]
        [HttpPut("decline-assignment/{id}")]
        public async Task<IActionResult> DeclineAssignment(string id)
        {

            var assignmentToDecline = await _deliveryAssignmentService.GetAssignmentByIdAsync(id);
            if (assignmentToDecline == null)
            {
                ModelState.AddModelError("Assignment", $"Assignment with {id} does not exist");
                return NotFound(Utilities.CreateResponse($"Assignment with {id} does not exist", ModelState, ""));
            }

            if (assignmentToDecline.Status == (byte)(DeliveryAssignmentStatus.Declined))
            {
                return Content("Assignment already declined");
            }
            assignmentToDecline.Status = (byte)(DeliveryAssignmentStatus.Declined);
            var response = await _deliveryAssignmentService.DeclineAssignmentById(assignmentToDecline);
            if (!response)
            {
                ModelState.AddModelError("Assignment", $"Assignment could not be declined");
                return BadRequest(Utilities.CreateResponse("Assignment could not be declined", ModelState, ""));
            }
            return Ok(Utilities.CreateResponse("Assignment declined", null, ""));
        }


        [Authorize(Roles = "Admin, Customer")]
        [HttpPost("add-assignment")]
        public async Task<IActionResult> AddAssignment([FromBody] AddAssignmentDto assignmentdto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            // Check if order Id exists if not return Bad Request
            var checkOrder = await _orderService.GetOrderByIdAsync(assignmentdto.OrderId);

            if (checkOrder == null)
            {
                ModelState.AddModelError("Order does not exist", $"Order with id {assignmentdto.OrderId} does not exist");

                return BadRequest(Utilities.CreateResponse<AssignmentResponseDto>("Assignment not successfully added", ModelState, null));
            }

            // Check if any Assignment contains the order Id return Bad Request
            var assignmentWithOrderId = await _deliveryAssignmentService.GetAssignmentsByOrderAsync(1, 2, assignmentdto.OrderId);
            if (assignmentWithOrderId.Count() != 0)
            {
                ModelState.AddModelError("Order already assigned", $"Order with id {assignmentdto.OrderId} has been assigned.");

                return BadRequest(Utilities.CreateResponse<AssignmentResponseDto>("Assignment not successfully added", ModelState, null));
            }


            var assignmentToAdd = _mapper.Map<AddAssignmentDto, DeliveryAssignment>(assignmentdto);
            assignmentToAdd.DateCreated = DateTime.Now.ToString();
            assignmentToAdd.DateUpdated = DateTime.Now.ToString();

            bool isSuccess = await _deliveryAssignmentService.AddAssignmentAsync(assignmentToAdd);

            if (!isSuccess)
            {
                ModelState.AddModelError("Unsuccessful", "Assignment not successfully added");

                return BadRequest(Utilities.CreateResponse<AssignmentResponseDto>("Assignment not successfully added", ModelState, null));
            }

            var assignmentNowStored = await _deliveryAssignmentService.GetAssignmentByIdAsync(assignmentToAdd.Id);
            var assignmentResponse = _mapper.Map<DeliveryAssignment, AssignmentResponseDto>(assignmentNowStored);

            return Ok(Utilities.CreateResponse("Successfully added new assignment", null, assignmentResponse));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-assignment/{assignmentId}")]
        public async Task<IActionResult> UpdateAssignment([FromBody] UpdateAssignmentDto assignmentdto, string assignmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var deliveryPerson = await _userManager.Users.Include(x => x.DeliveryPerson).FirstOrDefaultAsync(x => x.DeliveryPerson.Id == assignmentdto.DeliveryPersonId);

            if (deliveryPerson == null)
            {
                ModelState.AddModelError("Not found", $"Delivery person with id {assignmentdto.DeliveryPersonId} does not exist");
                return NotFound(Utilities.CreateResponse(message: "Unsuccessfully updated", errs: ModelState, data: ""));
            }


            var assignmentToUpdate = await _deliveryAssignmentService.GetAssignmentByIdAsync(assignmentId);

            if (assignmentToUpdate == null)
            {
                ModelState.AddModelError("Assignment Not found", $"Assignment with id {assignmentId} does not exist");
                return NotFound(Utilities.CreateResponse(message: $"Unsuccessfully updated", errs: ModelState, data: ""));
            }

            assignmentToUpdate.DeliveryPersonId = assignmentdto.DeliveryPersonId;
            assignmentToUpdate.Status = assignmentdto.Status;
            assignmentToUpdate.DateUpdated = DateTime.Now.ToString();


            bool isSuccess = await _deliveryAssignmentService.UpdateAssignmentAsync(assignmentToUpdate);

            if (!isSuccess)
            {
                ModelState.AddModelError("Unsuccessfully updated", "Assignment not successfully updated. Try again later.");

                return BadRequest(Utilities.CreateResponse("Assignment not successfully updated", errs: ModelState, data: ""));
            }


            return Ok(Utilities.CreateResponse("Successfully updated!", null, ""));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-assignment/{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(string assignmentId)
        {

            var assignmentToDelete = await _deliveryAssignmentService.GetAssignmentByIdAsync(assignmentId);

            if (assignmentToDelete == null)
            {
                ModelState.AddModelError("Assignment Error", $"Assignment with id {assignmentId} does not exist");
                return NotFound(Utilities.CreateResponse(message: "Assignment does not exist", errs: ModelState, data: ""));
            }

            bool isSuccess = await _deliveryAssignmentService.DeleteAssignmentAsync(assignmentToDelete);

            if (!isSuccess)
            {
                ModelState.AddModelError("Try again", "Please try again later");
                return BadRequest(Utilities.CreateResponse(message: "Operation not successful. Try again", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse("Successfully deleted", null, ""));
        }

        [Authorize(Roles = "Delivery")]
        [HttpPut("accept-assignment/{assignmentId}")]
        public async Task<IActionResult> AcceptAssignment(string assignmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }
            var user = await _userManager.GetUserAsync(User);

            var deliveryPerson = await _userManager.Users.Include(x => x.DeliveryPerson).FirstOrDefaultAsync(x => x.DeliveryPerson.AppUserId == user.Id);

            var assignmentToAccept = await _deliveryAssignmentService.GetAssignmentByIdAsync(assignmentId);

            if (assignmentToAccept == null)
            {
                ModelState.AddModelError("Not found", $"Assignment with id {assignmentId} does not exist.");
                return NotFound(Utilities.CreateResponse(message: $"Assignment with id {assignmentId} does not exist", errs: ModelState, data: ""));
            }

            if (assignmentToAccept.Status == 1)
            {
                ModelState.AddModelError("Assignment already accepted", $"This assignment is accepted already");
                return BadRequest(Utilities.CreateResponse(message: "This assignment is already accepted", errs: ModelState, data: ""));
            }

            assignmentToAccept.DeliveryPersonId = deliveryPerson.DeliveryPerson.Id;
            assignmentToAccept.Status = 1;
            assignmentToAccept.DateUpdated = DateTime.Now.ToString();

            bool isSuccess = await _deliveryAssignmentService.UpdateAssignmentAsync(assignmentToAccept);

            if (!isSuccess)
            {
                ModelState.AddModelError("Try again", "There appears to be an error, please try again.");
                return BadRequest(Utilities.CreateResponse(message: "Unsuccessful request", errs: ModelState, data: ""));
            }

            var assignmentResponse = _mapper.Map<DeliveryAssignment, AssignmentResponseDto>(assignmentToAccept);

            return Ok(Utilities.CreateResponse(message: "Assignment accepted", null, data: assignmentResponse));
        }



        [HttpGet("get-assignments-by-user-and-status")]
        [Authorize(Roles = "Admin, Delivery")]
        public async Task<IActionResult> GetAssignmentsByDeliveryPersonAndStatus([FromQuery] int page, [FromQuery] string AppUserId, [FromQuery] string status)
        {
            if (string.IsNullOrEmpty(AppUserId) || string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("Assignment", "An empty Delivery Person Id was passed");
                return BadRequest(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>("An empty Delivery Person Id was passed", errs: ModelState, null));
            }

            if (!Enum.IsDefined(typeof(DeliveryAssignmentStatus), Utilities.ChangeToTitleCase(status)))
            {
                ModelState.AddModelError("Assignment", "Not a valid Delivery Assignment status");
                return BadRequest(Utilities.CreateResponse<ShapedListOfDeliveryAssignment>("No assignment was found", errs: ModelState, null));
            }

            // Converts the string to Enum a Delivery assignment enum
            var enumStatus = (DeliveryAssignmentStatus)Enum
                        .Parse(typeof(DeliveryAssignmentStatus), status, true);


            page = page <= 0 ? 1 : page;
            var LoggedInUser = await _userManager.GetUserAsync(User);
            IEnumerable<ShapedListOfDeliveryAssignment> assignments;
            if (await _userManager.IsInRoleAsync(LoggedInUser, "Admin"))
            {
                assignments = await _deliveryAssignmentService.GetAssignmentsByStatusAsync(page, _perPage, (byte)enumStatus);
            }
            else
            {
                assignments = await _deliveryAssignmentService.GetAssgnmentByUserAndStatus(page, _perPage, LoggedInUser.Id, (byte)enumStatus);
            }

            var pagedItems = AddPaginationToResult(assignments, page);

            return Ok(Utilities.CreateResponse("All delivery assignment by delivery person", errs: null, data: pagedItems));
        }

    }

}
