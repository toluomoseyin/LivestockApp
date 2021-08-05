using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingHistory;
        private readonly IMapper _mapper;
        private readonly int _perPage;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderService _orderService;

        public TrackingController(ITrackingService trackingHistory, IMapper mapper, IConfiguration configuration,
                                  UserManager<AppUser> userManager, IOrderService orderService)
        {
            _trackingHistory = trackingHistory;
            _mapper = mapper;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
            _userManager = userManager;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("get-trackings/{id}")]
        public async Task<IActionResult> GetTrackingHistoryById(string id)
        {
            var trackingHistory = await _trackingHistory.GetTrackingHistoryByIdAsync(id);
            if (trackingHistory == null)
            {
                ModelState.AddModelError("Not found", $"No result found for Id {id}");
                return NotFound(Utilities.CreateResponse(message: "tracking Id does not exit", errs: ModelState, ""));
            }
            var trackingToReturn = _mapper.Map<TrackingHistoryDto>(trackingHistory);

            return Ok(Utilities.CreateResponse(message: "Order details", errs: null, data: trackingToReturn));
        }

        [Authorize]
        [HttpGet("get-trackings-by-order")]
        public async Task<IActionResult> GetTrackingHistoriesByOrder([FromQuery] string orderId, int page)
        {
            page = page <= 0 ? 1 : page;
            var trackingHistory = await _trackingHistory.GetTrackingHistoriesByOrderAsync(orderId, page, _perPage);
            if (trackingHistory.Count < 1)
            {
                ModelState.AddModelError("Not found", $"No result found for OrderId {orderId}");
                return NotFound(Utilities.CreateResponse(message: "No tracking history found for this order", errs: ModelState, ""));
            }

            var trackingToReturn = _mapper.Map<ICollection<TrackingHistoryDto>>(trackingHistory);

            return Ok(Utilities.CreateResponse(message: "Order details", errs: null, trackingToReturn));
        }

        [Authorize]
        [HttpGet("get-trackings-by-personId")]
        public async Task<IActionResult> GetTrackingHistoriesByDeliveryPerson([FromQuery] string personId, int page)
        {
            page = page <= 0 ? 1 : page;
            var trackingHistory = await _trackingHistory.GetTrackingHistoriesByDeliveryPersonAsync(personId, page, _perPage);
            if (trackingHistory.Count < 1)
            {
                ModelState.AddModelError("Not found", $"No result found for DeliveryPerson {personId}");
                return NotFound(Utilities.CreateResponse(message: "No tracking history found", errs: ModelState, ""));
            }

            var trackingToReturn = _mapper.Map<ICollection<TrackingHistoryDto>>(trackingHistory);

            return Ok(Utilities.CreateResponse(message: "Order details", errs: null, trackingToReturn));
        }

        [Authorize(Roles = "Delivery")]
        [HttpDelete("delete-tracking/{trackingId}")]
        public async Task<IActionResult> DeleteTrackingHistory(string trackingId)
        {
            var trackingToDelete = await _trackingHistory.GetTrackingHistoryByIdAsync(trackingId);
            if (trackingToDelete == null)
            {
                ModelState.AddModelError("Not found", $"No result found for Id {trackingId}");
                return NotFound(Utilities.CreateResponse(message: "No tracking history found", errs: ModelState, ""));
            }

            var deleteSuccess = await _trackingHistory.DeleteTrackingHistoryAsync(trackingId);
            if (!deleteSuccess)
            {
                ModelState.AddModelError("Not deleted", $"Unable to delete tracking with Id {trackingId}");
                return NotFound(Utilities.CreateResponse(message: "Tracking history not deleted", errs: ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "Successfully Deleted history", errs: null, data: deleteSuccess));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-tracking")]
        public async Task<IActionResult> GetAllTrackingHistories([FromQuery] int page)
        {
            page = page <= 0 ? 1 : page;
            var trackings = await _trackingHistory.GetTrackingHistoriesAsync(page, _perPage);
            if (trackings.Count() < 1)
            {
                ModelState.AddModelError("Not Found", "Tracking History is Empty");
                return NotFound(Utilities.CreateResponse<string>("No Tracking Found", ModelState, ""));
            }
            var trackingToReturn = _mapper.Map<IEnumerable<TrackingHistoryDto>>(trackings);
            var pageMetaData = Utilities.Paginate(page, _perPage, _trackingHistory.GetCount());
            var pagedItems = new PaginatedResultDto<TrackingHistoryDto> { PageMetaData = pageMetaData, ResponseData = trackingToReturn };
            return Ok(Utilities.CreateResponse("All Tracking History", null, pagedItems));


        }



        [Authorize(Roles = "Delivery")]
        [HttpPut("update-tracking/{trackingId}")]
        public async Task<IActionResult> UpdateTracking(string trackingId, [FromBody] UpdateTrackingDto updateTracking)
        {
            if (string.IsNullOrWhiteSpace(trackingId))
            {
                ModelState.AddModelError("Id", "Id must not be empty");
                return BadRequest(Utilities.CreateResponse("Id not provided", ModelState, ""));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse("Invalid model", ModelState, ""));
            }

            var trackHistory = await _trackingHistory.GetTrackingHistoryByIdAsync(trackingId);
            if (trackHistory == null)
            {
                ModelState.AddModelError("NotFound", "History does not exist");
                return NotFound(Utilities.CreateResponse("History not found", ModelState, ""));
            }

            var history = _mapper.Map(updateTracking, trackHistory);

            var isHistoryUpdated = await _trackingHistory.UpdateTrackingHistoryAsync(history);
            if (!isHistoryUpdated)
            {
                ModelState.AddModelError("UnSuccessful", "Update not successful");
                return BadRequest(Utilities.CreateResponse("Tracking history not updated", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse("Successfully updated tracking history", null, "")); 
        }


        [Authorize(Roles = "Delivery")]
        [HttpPost("add-tracking")]
        public async Task<IActionResult> AddTracking([FromBody] AddTrackingDto addTracking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse("Invalid model", ModelState, ""));
            }

            var deliveryPerson = await _userManager.Users.Include(x => x.DeliveryPerson).FirstOrDefaultAsync(x => x.DeliveryPerson.AppUserId == addTracking.UserId);

            
            if (deliveryPerson == null)
            {
                ModelState.AddModelError("UnAuthorize", "User is not authorized to access this route");
                return Unauthorized(Utilities.CreateResponse("User is not authorized ", ModelState, ""));
            }

            var orderToTrack = await _orderService.FindOrderByIdAsync(addTracking.OrderId);
            if (orderToTrack == null)
            {

                ModelState.AddModelError("NotFound", "Order to Track Does not Exist");
                return NotFound(Utilities.CreateResponse($"Order to Track with id {addTracking.OrderId } Does not Exist ", ModelState, ""));

            }
            // update delivery person Id here
            addTracking.DeliveryPersonId = deliveryPerson.DeliveryPerson.Id;
            var trackHistory = _mapper.Map<TrackingHistory>(addTracking);
            var response = await _trackingHistory.AddTrackingHistoryAsync(trackHistory);

            if (!response)
            {
                ModelState.AddModelError("Tracking history", "Tracking history could not be added");
                return BadRequest(Utilities.CreateResponse("Error adding tracking history", ModelState, ""));
            }

            return Created("", Utilities.CreateResponse("Successfully added new history", null, ""));

        }


        [Authorize]
        [HttpGet("get-tracking-by-num")]
        public async Task<IActionResult> GetTrackingByTrackingNumber([FromQuery] string number)
        {
            if (string.IsNullOrEmpty(number.ToString()))
            {
                ModelState.AddModelError("EmptyTrackingNumber", "Id must not be empty");
                return BadRequest(Utilities.CreateResponse("Tracking Number not provided", ModelState, ""));
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            if(loggedInUser == null)
            {
                ModelState.AddModelError("UnAuthorized", "User is UnAuthorized");
                return BadRequest(Utilities.CreateResponse("User is UnAuthorized", ModelState, ""));
            }

            var trackingHistories = await _trackingHistory.GetTrackingHistoriesByTrackingNumberAsync(number, loggedInUser.Id);
            if(trackingHistories.Count() < 1)
            {
                ModelState.AddModelError("NotFound", "No Tracking Histories Found for this User");
                return NotFound(Utilities.CreateResponse("No Tracking History was found for this User", ModelState, ""));
            }

            var trackingHistoriesForUser = _mapper.Map<IEnumerable<TrackingHistoryForUsersDto>>(trackingHistories);
            return Ok(Utilities.CreateResponse("All Tracking History", null, trackingHistoriesForUser));
        }

    }
}
