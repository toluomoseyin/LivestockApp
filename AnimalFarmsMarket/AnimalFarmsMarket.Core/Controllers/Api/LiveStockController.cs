using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using static AnimalFarmsMarket.Commons.Utilities;
using Microsoft.AspNetCore.Identity;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[controller]")]
    public class LiveStockController : ControllerBase
    {
        private readonly ILiveStockService _livestock;
        private readonly IFileUpload _fileupload;
        private readonly IMapper _mapper;
        private readonly int _perPage;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRatingService _rating;
        private readonly IReviewService _review;

        public LiveStockController(ILiveStockService liveStockService, IFileUpload fileUpload, IMapper mapper,
            IConfiguration configuration, UserManager<AppUser> userManager, IRatingService rating, IReviewService reviewService)
        {
            _livestock = liveStockService;
            _fileupload = fileUpload;
            _mapper = mapper;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:PerPage").Value);
            _userManager = userManager;
            _rating = rating;
            _review = reviewService;
        }



        [Authorize(Roles = "Agent, Admin")]
        [HttpGet("get-livestocks")]
        public async Task<IActionResult> GetAllLiveStock([FromQuery] int page)
        {
            var _perPage = 9;
            page = page <= 0 ? 1 : page;
            IEnumerable<ShappedListOfLivestock> livestocks = null;
            var LoggedInUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(LoggedInUser, "Admin"))
            {
                livestocks = await _livestock.GetLiveStocksAndMainImageAsync(page, _perPage);
            }
            else
            {
                livestocks = await _livestock.GetLiveStocksAndMainImageByAgentAsync(page, _perPage, LoggedInUser.Id);
            }
            var pageMetaData = Utilities.Paginate(page, _perPage, _livestock.TotalCount);
            var pagedItems = new PaginatedResultDto<ShappedListOfLivestock> { PageMetaData = pageMetaData, ResponseData = livestocks };
            return Ok(Utilities.CreateResponse("List of livestocks paginated by 10", errs: null, data: pagedItems));
        }





        [HttpGet("get-livestocksLocation")]
        public IActionResult GetLivestockLocation()
        {
            var livestocks = _livestock.GetLiveStockLocation();

            if (livestocks == null || (livestocks.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No result found page"); // put the error you wish to see on the MVC View here...
                return NotFound(Utilities.CreateResponse(message: "livestocks not found", errs: ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "List of livestocks paginated by 10", errs: null, data: livestocks));
        }





        [Authorize(Roles = "Agent")]
        [HttpPatch]
        [Route("add-photo/{LivestockId}")]
        public async Task<IActionResult> UploadPhoto([FromForm] IFormFile LivestockPhoto, string LivestockId)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            if (!activeUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (!ModelState.IsValid)
            {
                var responseObj = Utilities.CreateResponse("Model state error", ModelState, "");
                return BadRequest(responseObj);
            }

            if (string.IsNullOrWhiteSpace(LivestockId))
            {
                ModelState.AddModelError("Id", "Id is not valid");
                var responseObj = Utilities.CreateResponse("Invalid Id", ModelState, "");
                return BadRequest(responseObj);
            }

            // Checking for valid image format
            var vetPic = PictureFilter.CheckPictureTypeAndSize(LivestockPhoto);
            if (vetPic == "SizeError")
                return BadRequest("Image size is not valid");

            if (vetPic == null)
                return BadRequest("Could not add livestock image");

            var livestock = await _livestock.GetLivestockFullDetailsByIdAsync(LivestockId);
            if (livestock == null)
            {
                ModelState.AddModelError("Not found", "No result found"); // put the error you wish to see on the MVC View here...
                return NotFound(Utilities.CreateResponse(message: "livestocks not found", errs: ModelState, ""));
            }

            var uploadAvatarResponse = _fileupload.UploadAvatar(LivestockPhoto);

            LivestockImages livestockImages = new LivestockImages
            {
                ImageUrl = uploadAvatarResponse.AvatarUrl, //"sjflsnfjskdns"
                PublicId = uploadAvatarResponse.PublicId, //"skdfkdgkdfgndkfgdf"
                LivestockId = LivestockId,
                IsMain = (livestock.Images.Any(x => x.IsMain == true)) ? false : true
            };

            var livestockImg = await _livestock.AddLivestockImageAsync(livestockImages);

            if (!livestockImg)
            {
                ModelState.AddModelError("Failed to add", "Could not add livestock image"); // put the error you wish to see on the MVC View here...
                return NotFound(Utilities.CreateResponse(message: "Could not add livestock image", errs: ModelState, ""));
            }

            var response = new LiveStockPhotoDto
            {
                ImageUrl = livestockImages.ImageUrl,
                PublicId = livestockImages.PublicId,
            };

            return Ok(Utilities.CreateResponse("Photo successfully added for livestock", errs: null, data: response));
        }


        [Authorize(Roles = "Agent")]
        [HttpDelete]
        [Route("delete-photo/{livestockId}")]
        public async Task<IActionResult> DeleteLiveStockPhoto(string livestockId, [FromBody] LivestockImageToDeleteDto model)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            if (!activeUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (string.IsNullOrEmpty(livestockId) && string.IsNullOrEmpty(model.publicId))
            {
                ModelState.AddModelError("Invalid Id", "Ensure ids are added");
                return BadRequest(Utilities.CreateResponse("errors", ModelState, ""));
            }

            var livestock = await _livestock.GetLivestockImageAsync(livestockId, model.publicId);

            if (livestock == null)
            {
                ModelState.AddModelError("Not found", $"No result found for Id: {livestockId} and publicId: {model.publicId}");
                return NotFound(Utilities.CreateResponse("Not found", ModelState, ""));
            }

            var result = await _livestock.DeleteLivestockImageAsync(livestockId, model.publicId);
            if (!result)
            {
                ModelState.AddModelError("Delete failed", "Failed to delete image from db");
                return BadRequest(Utilities.CreateResponse("Could not delete from the database", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "Livestock photo successfully deleted!", errs: null, data: ""));

        }



        [Authorize(Roles = "Agent")]
        [HttpDelete]
        [Route("delete-photos/{Id}")]
        public async Task<IActionResult> DeleteLiveStockPhotos(string Id, string[] publicIds)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            if (!activeUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            if (string.IsNullOrEmpty(Id) || publicIds.Length < 1)
            {
                ModelState.AddModelError("Invalid Id", "Ensure ids are added");
                return BadRequest(Utilities.CreateResponse("errors", ModelState, ""));
            }

            var result = await _livestock.DeleteAllLivestockImagesAsync(Id);
            if (!result)
            {
                ModelState.AddModelError("Db Delete failed", "Failed to delete images from db");
                return BadRequest(Utilities.CreateResponse("Not found", ModelState, ""));
            }

            var errs = new Dictionary<string, string>();
            foreach (var publicId in publicIds)
            {
                var deletionResult = _fileupload.DeleteAvatar(publicId);

                if (deletionResult.Error != null)
                {
                    errs.Add($"Delete failed {publicId}", $"Failed to delete from cloudinary for public id: {publicId}");
                }
            }
            if (errs.Count > 0)
            {
                foreach (var err in errs)
                {
                    ModelState.AddModelError(err.Key, err.Value);
                }
                return BadRequest(Utilities.CreateResponse("Cloudnary error", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "Livestock photos successfully deleted!", errs: null, data: ""));

        }



        [HttpGet]
        [Route("get-livestocks-by-search-term")]
        public async Task<IActionResult> GetByQueryParams([FromQuery] SearchLivestockDto query)
        {
            if (!ModelState.IsValid)
                return BadRequest(Utilities.CreateResponse("Model state error", errs: ModelState, data: ""));

            query.PageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            var livestocks = await _livestock.GetLivestocksByQueriesAsync(query, _perPage);

            if (livestocks == null || (livestocks.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No result found page {query.PageNumber}"); 
                return NotFound(Utilities.CreateResponse(message: "livestocks not found", errs: ModelState, ""));
            }

            var pageMetaData = Utilities.Paginate(query.PageNumber, _perPage, _livestock.TotalCount);
            var pagedItems = new PaginatedResultDto<ShappedListOfLivestock> { PageMetaData = pageMetaData, ResponseData = livestocks };
            return Ok(Utilities.CreateResponse(message: "All Livestocks", errs: null, data: pagedItems));

        }





        [HttpGet]
        [Route("get-livestocks-by-location-and-market")]
        public async Task<IActionResult> GetByLocationAndMarket(int page, string location, string market)
        {

            page = page <= 0 ? 1 : page;

            var _perPage = 9;

            var livestocks = await _livestock.GetLivestocksByLocationAndMarketAsync(page, _perPage, location, market);

            if (livestocks == null || (livestocks.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No result found page {page}"); // put the error you wish to see on the MVC View here...
                return NotFound(Utilities.CreateResponse(message: "livestocks not found", errs: ModelState, ""));
            }

            var pageMetaData = Utilities.Paginate(page, _perPage, _livestock.TotalCount);
            var pagedItems = new PaginatedResultDto<ShappedListOfLivestock> { PageMetaData = pageMetaData, ResponseData = livestocks };
            return Ok(Utilities.CreateResponse(message: "All Livestocks", errs: null, data: pagedItems));
        }





        [HttpGet]
        [Route("get-livestocks-by-location")]
        public async Task<IActionResult> GetByLocation(int page, string location)
        {
            page = page <= 0 ? 1 : page;

            var livestocks = await _livestock.GetLivestocksByLocationAsync(page, _perPage, location);
            if (livestocks == null || (livestocks.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No result found page {page}"); // put the error you wish to see on the MVC View here...
                return NotFound(Utilities.CreateResponse(message: "livestocks not found", errs: ModelState, ""));
            }

            var pageMetaData = Utilities.Paginate(page, _perPage, _livestock.TotalCount);
            var pagedItems = new PaginatedResultDto<ShappedListOfLivestock> { PageMetaData = pageMetaData, ResponseData = livestocks };
            return Ok(Utilities.CreateResponse(message: "All Livestocks", errs: null, data: pagedItems));
        }




        [HttpDelete]
        [Route("delete-livestock/{id}")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> DeleteLivestock(string id)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            if (!activeUser.IsActive)
            {
                ModelState.AddModelError("Access denied", "In-active user");
                var responseObj = Utilities.CreateResponse("Access denied for in-active user", ModelState, "");
                return BadRequest(responseObj);
            }

            var livestock = await _livestock.GetLivestockFullDetailsByIdAsync(id);

            if (livestock == null)
            {
                ModelState.AddModelError("Not found", $"Livestock not found with id : {id}");
                var response = Utilities.CreateResponse("Livestock with provided id not found", ModelState, "");
                return NotFound(response);
            }

            if (livestock.Agent.AppUserId != activeUser.Id)
            {
                ModelState.AddModelError("Access denied", "You are not authorized to delete another agent's livestock");
                var responseObj = Utilities.CreateResponse("Access denied from deleting another agent's livestock", ModelState, "");
                return Unauthorized(responseObj);
            }

            if (!await _livestock.DeleteLivestockAsync(livestock))
            {
                ModelState.AddModelError("Livestock", "Could not successfully delete the livestock");
                return BadRequest(Utilities.CreateResponse("Livestock was not deleted", ModelState, ""));
            }

            var pIds = livestock.Images.Select(x => x.PublicId);

            var result = await DeleteLiveStockPhotos(id, pIds.ToArray());

            var responseOb = Utilities.CreateResponse("Deleted successfully", ModelState, "");
            return Ok(responseOb);
        }




        [Authorize(Roles = "Agent")]
        [Route("add-livestock")]
        [HttpPost]
        public async Task<IActionResult> AddLivestock([FromBody] LivestockToAdd livestockToAdd)
        {
            if (!ModelState.IsValid)
            {
                var responseObject = Utilities.CreateResponse("Invalid livestock request", errs: ModelState, data: "");
                return BadRequest(responseObject);
            }

            var livestockEntity = _mapper.Map<Livestock>(livestockToAdd);

            if (!await _livestock.AddLivestockAsync(livestockEntity))
            {
                ModelState.AddModelError("Livestock", "Coulld not add livestock successfully");
                var responseOb = Utilities.CreateResponse("Could not add livestock successfully", ModelState, "");
                return UnprocessableEntity(responseOb);
            }

            var response = Utilities.CreateResponse("Livestock added successfully", null, livestockEntity.Id);

            return StatusCode(201, response);
        }




        [HttpGet]
        [Route("get-livestock/{id}")]
        public async Task<IActionResult> GetLivestock(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, ""));
            }

            var livestock = await _livestock.GetLivestockFullDetailsByIdAsync(id);

            //Ensure livestock exists
            if (livestock == null)
            {
                ModelState.AddModelError("Not found", "LivestockId does not exist");
                var resp = Utilities.CreateResponse(message: "Invalid LivestockId", errs: ModelState, data: "");
                return NotFound(resp);
            }

            foreach (var review in livestock.Reviews)
            {
                review.User = await _review.GetReviewerOnALivestock(review.LivestockId, review.UserId);
            }

            // map big object
            var res = _mapper.Map<LivestockResponseDto>(livestock);
            res.LivestockAgent = _mapper.Map<UserDto>(livestock.Agent.AppUser);
            res.LivestockAgent.Id = livestock.AgentId;
            res.LivestockAgent.BusinessLocation = livestock.Agent.BusinessLocation;
            res.LivestockMarket = _mapper.Map<MarketDto>(livestock.Market);
            res.LivestockMarket.Address = _mapper.Map<MarketAddressDto>(livestock.Market.MarketAddress);
            res.Rating = await _rating.GetRating(livestock.Id);
            res.Reviews = _mapper.Map<IEnumerable<ReviewDto>>(livestock.Reviews);

            // Generate return object and return appropraite data
            var msg = Utilities.CreateResponse("Livestock detail", null, res);
            return Ok(msg);
        }




        [Authorize(Roles = "Agent")]
        [HttpPut]
        [Route("update-livestock/{livestockId}")]
        public async Task<IActionResult> UpdateLivestock(string livestockId, [FromBody] LivestockUpdateDto livestockRequest)
        {
            if (string.IsNullOrWhiteSpace(livestockId))
            {
                ModelState.AddModelError("Invalid Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, ""));
            }

            // Ensure Model is in valid state
            if (!ModelState.IsValid)
            {
                var msg = Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            // Ensure livestock to update exists            
            var livestock = await _livestock.GetLivestockByIdAsync(livestockId);
            if (livestock == null)
            {
                ModelState.AddModelError("Not found", "LivestockId does not exist");
                var msg = Utilities.CreateResponse(message: "Invalid LivestockId", errs: ModelState, data: "");
                return NotFound(msg);
            }

            //map livestock for update
            livestock = _mapper.Map<Livestock>(livestockRequest);
            livestock.Id = livestockId;


            // Ensure update was successful
            var res = await _livestock.UpdateLivestock(livestock);
            if (!res)
            {
                ModelState.AddModelError("Not updated", "update not successfull");
                var msg = Utilities.CreateResponse(message: "Livestock not Updated", errs: ModelState, data: "");
                return BadRequest(msg);
            }

            return Ok(Utilities.CreateResponse(message: "Livestock Updated successfully", errs: null, data: ""));
        }





        [HttpGet("get-livestock-category-data")]

        public IActionResult GetLivestockCategoryData(string category)
        {


            var livestocks = _livestock.GetBreedsSexesAndWeightsForLivestocksCategory(category);

            if (livestocks == null || (livestocks.Count() < 1))
            {
                ModelState.AddModelError("Not found", $"No livestock found for this category");
                return NotFound(Utilities.CreateResponse(message: "livestocks not found", errs: ModelState, ""));
            }


            return Ok(Utilities.CreateResponse(message: "List of livestocks data", errs: null, data: livestocks));
        }

        [HttpGet("get-markets")]
        public async Task<IActionResult> GetMarkets()
        {
            var markets = await _livestock.GetMarketsAsync();
            var marketsToReturn = _mapper.Map<IEnumerable<AddLivestockMarketDto>>(markets);
            return Ok(Utilities.CreateResponse(message: "Markets Retrival Successful", errs: null, data: marketsToReturn));
        }


        [HttpGet("get-agent-livestocks")]
        [Authorize(Roles = "Agent, Admin")]

        public async Task<IActionResult> GetAgentLivestocks([FromQuery] string agentid)
        {
            if (string.IsNullOrWhiteSpace(agentid))
            {
                ModelState.AddModelError("Invalid Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, ""));
            }
            var livestocks = await _livestock.GetAgentLivestocksAndMarkets(agentid);

            //var livestockToReturn = _mapper.Map<IEnumerable<AgentLivestockDto>>(livestocks);
            var livestockToReturn = new List<LivstockMarketDto>();
            
            foreach(var item in livestocks)
            {
               
                var l = new LivstockMarketDto()
                {
                    MarketName = item.Key,
                    AgentName = $"{item.ToList()[0].Agent.AppUser.FirstName} {item.ToList()[0].Agent.AppUser.LastName}",
                    Livestocks = _mapper.Map<List<LivestockToReturnDto>>(item.ToList())
                };
                livestockToReturn.Add(l);


            }


            return Ok(Utilities.CreateResponse(message: "livestock Retrival Successful", errs: null, data: livestockToReturn));


        }

        [HttpPost("restock-livestock")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> RestockLivestock([FromBody] RestockLivestockDto restockLivestockDto)
        {
            if (string.IsNullOrWhiteSpace(restockLivestockDto.LivestockId))
            {
                ModelState.AddModelError("Invalid Id", "Id must be provided");
                return BadRequest(Utilities.CreateResponse("Id was not provided", ModelState, ""));
            }

            var livestock = await _livestock.GetLivestockByIdAsync(restockLivestockDto.LivestockId);

            var updatedLivestock = _mapper.Map(restockLivestockDto,livestock);

            var res = await _livestock.UpdateLivestock(updatedLivestock);

            if (!res)
            {
                return BadRequest(Utilities.CreateResponse("Could not restock livestock", ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "Livestock Restock successful", errs: null, data:""));
        }
    }
}
