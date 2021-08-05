using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILiveStockService _livestockService;
        private readonly IMapper _mapper;
        private readonly int _perPage;

        public ReviewController(IReviewService reviewService, UserManager<AppUser> userManager, ILiveStockService livestockService, IMapper mapper, IConfiguration configuration)
        {
            _reviewService = reviewService;
            _userManager = userManager;
            _livestockService = livestockService;
            _mapper = mapper;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
        }

        [HttpGet("get-review/{reviewId}")]
        public async Task<IActionResult> GetReviewById(string reviewId)
        {
            if (string.IsNullOrWhiteSpace(reviewId))
            {
                ModelState.AddModelError("Id", "Invalid Request");
                var response = Utilities.CreateResponse("Invalid Request", ModelState, string.Empty);
                return BadRequest(response);
            }
            
            var review = await _reviewService.GetReviewAsync(reviewId);

            if (review == null)
            {
                ModelState.AddModelError("Review", "review does not exist");
                var response = Utilities.CreateResponse("review does not exist", ModelState, string.Empty);
                return BadRequest(response);
            }

            var reviewDto = _mapper.Map<ReviewToReturnDto>(review);

            return Ok(Utilities.CreateResponse("", null, reviewDto));
        }

        [HttpGet("get-reviews/{livestockId}")]
        public async Task<IActionResult> GetAllReviewsByLiveStock([FromRoute] string livestockId, [FromQuery] int page = 1)
        {
            if (string.IsNullOrWhiteSpace(livestockId))
            {
                ModelState.AddModelError("Id", "Invalid Request");
                return BadRequest(Utilities.CreateResponse("Invalid Request", ModelState, string.Empty));
            }

            if (page < 1) page = 1;

            if (await _livestockService.GetLivestockByIdAsync(livestockId) == null)
            {
                ModelState.AddModelError("Livestock", "livestock does not exist");
                var res = Utilities.CreateResponse("livestock does not exist", ModelState, string.Empty);
                return NotFound(res);
            }
            
            var reviews = await _reviewService.GetReviewsByLiveStockAsync(livestockId, page, _perPage);

            var paginatedReviews = PaginateReviews(reviews, page);

            var response = Utilities.CreateResponse("list of reviews", null, paginatedReviews);
            
            return Ok(response);
        }

        [HttpGet("get-reviews/user/{userId}")]
        public async Task<IActionResult> GetAllReviewsByUser([FromRoute] string userId, [FromQuery] int page = 1)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(Utilities.CreateResponse("Invalid Request", ModelState, string.Empty));

            if (page < 1) page = 1;

            if (await _userManager.FindByIdAsync(userId) == null)
            {
                ModelState.AddModelError("User", "user does not exist");
                var res = Utilities.CreateResponse("user does not exist", ModelState, string.Empty);
                return NotFound(res);
            }
            
            var reviews = await _reviewService.GetReviewsByUserAsync(userId, page, _perPage);

            var paginatedReviews = PaginateReviews(reviews, page);

            var response = Utilities.CreateResponse("list of reviews", null, paginatedReviews);
            
            return Ok(response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("add-review")]
        public async Task<IActionResult> AddReview([FromBody] ReviewToAddDto dto)
        {
            if (!ModelState.IsValid)
            {
                var response = Utilities.CreateResponse("Invalid Request", ModelState, string.Empty);
                return BadRequest(response);
            }

            var reviewByUser = _reviewService.CheckReviewByUserAsync(dto.UserId, dto.LivestockId);
            if (reviewByUser.Result != null)
            {
                ModelState.AddModelError("Review", "User can only have one review");
                var response = Utilities.CreateResponse("User can only review once", ModelState, string.Empty);
                return BadRequest(response);
            }

            var review = _mapper.Map<Review>(dto);

            var result = await _reviewService.AddReviewAsync(review);

            if (!result)
            {
                ModelState.AddModelError("ReviewText", "Adding review was not successful");
                var response = Utilities.CreateResponse("Adding review was not successful", ModelState, string.Empty);
                return BadRequest(response);
            }

            return StatusCode(201, Utilities.CreateResponse("successfully add new review", null, string.Empty));
        }

        [HttpPatch("update-review/{reviewId}")]
        public async Task<IActionResult> UpdateReview([FromRoute] string reviewId, [FromBody] UpdateReviewDto reviewDto)
        {
            if (string.IsNullOrWhiteSpace(reviewId))
            {
                ModelState.AddModelError("Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, string.Empty));
            }

            if (!ModelState.IsValid)
                return BadRequest(Utilities.CreateResponse("Model state error", ModelState, string.Empty));

            var review = await _reviewService.GetReviewAsync(reviewId);
            if (review == null)
            {
                ModelState.AddModelError("Review", "review does not exist");
                var response = Utilities.CreateResponse("review does not exist", ModelState, string.Empty);
                return NotFound(response);
            }

            review.ReviewText = reviewDto.ReviewText;
            review.DateUpdated = DateTime.Now.ToString();

            var result = await _reviewService.UpdateReviewAsync(review);
            if (!result)
            {
                ModelState.AddModelError("Review", "Could not update review");
                return BadRequest(Utilities.CreateResponse("Could not update review", ModelState, string.Empty));
            }
            
            return StatusCode(204, Utilities.CreateResponse(message: "successfully updated review", null, string.Empty));
        }

        [HttpDelete("delete-review/{reviewId}")]
        public async Task<IActionResult> DeleteReview([FromRoute] string reviewId)
        {
            if (string.IsNullOrWhiteSpace(reviewId))
            {
                ModelState.AddModelError("Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, string.Empty));
            }

            var result = await _reviewService.DeleteReview(reviewId);

            if (!result)
            {
                ModelState.AddModelError("Review", "Invalid Id passed");
                return BadRequest(Utilities.CreateResponse("Could not delete review. Id provided invalid", ModelState, string.Empty));
            }

            return StatusCode(204, Utilities.CreateResponse(message: "successfully deleted review", null, string.Empty));
        }
        
        private PaginatedResultDto<ReviewToReturnDto> PaginateReviews(IEnumerable<Review> reviews, int page)
        {
            var mappedReviews = _mapper.Map<IEnumerable<ReviewToReturnDto>>(reviews);

            var paginate = Utilities.Paginate(page, _perPage, _reviewService.TotalCount);
            
            return new PaginatedResultDto<ReviewToReturnDto>
            {
                ResponseData = mappedReviews, 
                PageMetaData = paginate
            };
        }
    }
}