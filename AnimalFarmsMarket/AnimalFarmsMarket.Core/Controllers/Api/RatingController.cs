using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[Controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _rating;
        private readonly int _perPage;
        private readonly IMapper _mapper;

        public RatingController(IMapper mapper, IRatingService rating, IConfiguration configuration)
        {
            _mapper = mapper;
            _rating = rating;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:PerPage").Value);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("add-rating")]
        public async Task<IActionResult> AddRatingAsync([FromBody] AddRatingDto ratingToAdd)
        {
            if (!ModelState.IsValid)
            {
                var response = Utilities.CreateResponse("Model state error", ModelState, data: "");
                return BadRequest(response);
            }

            var ratingByUser = _rating.CheckRatingByUserAsync(ratingToAdd.UserId, ratingToAdd.LivestockId);
            if (ratingByUser.Result != null)
            {
                ModelState.AddModelError("Rating", "User can only have one rating");
                var response = Utilities.CreateResponse("User can only rate once", ModelState, string.Empty);
                return BadRequest(response);
            }

            var mappedRating = _mapper.Map<Rating>(ratingToAdd);

            if (!await _rating.AddRatingAsync(mappedRating))
            {
                ModelState.AddModelError("Rating", "Rating was not successful");
                var response = Utilities.CreateResponse("Rating was not successful", ModelState, string.Empty);
                return UnprocessableEntity(response);
            }

            return Created("", Utilities.CreateResponse("successfully Added a new rating", null, ""));
        }

        [Authorize]
        [HttpGet("get-ratings-by-livestock/{livestockId}")]
        public async Task<IActionResult> GetRatingsByLiveStockAsync([FromQuery] int page, string livestockId)
        {
            page = page <= 0 ? 1 : page;

            var ratingsByLivestock = await _rating.GetRatingsByLivestockAsync(livestockId, page, _perPage);

            if (ratingsByLivestock.Count() == 0)
            {
                ModelState.AddModelError("Rating", "No Rating was made for the Requested livestock");
                return NotFound(Utilities.CreateResponse(message: "Rating not found", errs: ModelState, ""));
            }
            var mappedRatings = _mapper.Map<ICollection<GetRatingsDto>>(ratingsByLivestock);
            var response = AddPaginationToResult(mappedRatings, page);

            return Ok(Utilities.CreateResponse(message: "List of ratings", errs: null, data: response));
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("update-rating")]
        public async Task<IActionResult> UpdateRatingasync([FromBody] UpdateRatingDto newRating, [FromQuery] string ratingId)
        {
            if (!ModelState.IsValid)
            {
                var response = Utilities.CreateResponse("Invalid Request", ModelState, string.Empty);
                return BadRequest(response);
            }

            var ratingToUpdate = await _rating.GetRatingByIdAsync(ratingId);

            if (ratingToUpdate == null)
            {
                ModelState.AddModelError("Rating", "Rating was not found");
                return NotFound(Utilities.CreateResponse(message: "Rating not found", errs: ModelState, ""));
            }

            ratingToUpdate.RatingFigure = newRating.RatingFigure;

            var Updateresponse = await _rating.UpdateRatingAsync(ratingToUpdate);

            if (!Updateresponse)
            {
                ModelState.AddModelError("Rating", "Sorry could not update rating");
                return NotFound(Utilities.CreateResponse(message: "Rating not found", errs: ModelState, ""));
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("get-ratings-by-User/{UserId}")]
        public async Task<IActionResult> GetRatingsByUserAsync([FromQuery] int page, string UserId)
        {
            page = page <= 0 ? 1 : page;

            var ratingsByLivestock = await _rating.GetRatingsByUserAsync(UserId, page, _perPage);

            if (ratingsByLivestock.Count() == 0)
            {
                ModelState.AddModelError("Rating", "No Rating was made by this user");
                return NotFound(Utilities.CreateResponse(message: "Rating not found", errs: ModelState, ""));
            }
            var mappedRatings = _mapper.Map<ICollection<GetRatingsDto>>(ratingsByLivestock);
            var response = AddPaginationToResult(mappedRatings, page);

            return Ok(Utilities.CreateResponse(message: "List of ratings", errs: null, data: response));
        }


        [Authorize(Roles = "Customer,Admin")]
        [HttpDelete("delete-rating")]
        public async Task<IActionResult> DeleteRatingAsync([FromQuery] string ratingId)
        {
            if (string.IsNullOrWhiteSpace(ratingId))
            {
                ModelState.AddModelError("Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, string.Empty));
            }
            var result = await _rating.DeleteRatingAsync(ratingId);
            if (!result)
            {
                ModelState.AddModelError("Rating", "Invalid Id passed");
                return UnprocessableEntity(Utilities.CreateResponse("Could not delete Rating. Id provided is invalid", ModelState, string.Empty));
            }
            return NoContent();
        }


        private PaginatedResultDto<GetRatingsDto> AddPaginationToResult(IEnumerable<GetRatingsDto> Ratings, int page)
        {
            var pageMetaData = Utilities.Paginate(page, _perPage, _rating.GetCount());
            var pagedItems = new PaginatedResultDto<GetRatingsDto> { PageMetaData = pageMetaData, ResponseData = Ratings };
            return pagedItems;
        }

    }
}
