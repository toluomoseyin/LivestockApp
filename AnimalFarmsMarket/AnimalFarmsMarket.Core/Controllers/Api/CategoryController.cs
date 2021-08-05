using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly int _perPage;

        public CategoryController(ICategoryService categoryService, IConfiguration configuration)
        {
            _categoryService = categoryService;
            _perPage = Convert.ToInt32(configuration.GetSection("PaginationSettings:perPage").Value);
        }

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetAllCategories( [FromQuery] int page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            page = page <= 0 ? 1 : page;

            var categories = await _categoryService.GetCategoriesAsync(page, _perPage);
            var categoriesToReturn = new List<CategoryToReturnDto>();

            foreach (var item in categories)
            {
                var itemsToBeAdded = new CategoryToReturnDto
                {
                    Id = item.Id,
                    Name = item.Name
                };
                categoriesToReturn.Add(itemsToBeAdded);
            }

            var pageMetaData = Utilities.Paginate(page, _perPage, _categoryService.TotalCount);
            var pagedItems = new PaginatedResultDto<CategoryToReturnDto> { PageMetaData = pageMetaData, ResponseData = categoriesToReturn };
            return Ok(Utilities.CreateResponse(message: "All categories", errs: null, data: pagedItems));
        }

        

        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetCategory(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                ModelState.AddModelError("Category", "Category does not exist");
                return NotFound(Utilities.CreateResponse(message: "category not found", errs: ModelState, ""));
            }

            var response = new CategoryToReturnDto { Id = category.Id, Name = category.Name };
            return Ok(Utilities.CreateResponse(message: "category gotten by Id", errs: null, data: response));
        }


        /// <summary>
        /// Add Category using category name
        /// </summary>
        /// <param name="categoryToAddDto"></param>
        /// <returns></returns>
        //https://<base_url>/api/v1/Category/add-cartegory
        [Authorize(Roles = "Admin")]
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryToAddDto categoryToAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var category = new Category { Name = categoryToAddDto.CategoryName, Description = categoryToAddDto.CategoryDescription };
            var response = await _categoryService.AddCategoryAsync(category);

            if (!response)
            {
                ModelState.AddModelError("Category", "Could not add category");
                return BadRequest(Utilities.CreateResponse(message: "category not added", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse(message: "Added sucessfully", errs: null, data: "")); ;

        }

        /// <summary>
        /// Delete a single category by Category Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        [HttpDelete("delete-category/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                ModelState.AddModelError("Id", "Id is not provided");
                return BadRequest(Utilities.CreateResponse("Id must be provided", ModelState, ""));
            }

            var categoryToDelete = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (categoryToDelete == null)
            {
                ModelState.AddModelError("Id", "Id does not exist");
                return NotFound(Utilities.CreateResponse("No record found", ModelState, ""));
            }

            var categoryResponse = await _categoryService.DeleteCategoryAsync(categoryToDelete);
            if (!categoryResponse)
            {
                ModelState.AddModelError("Category", "Could not delete category");
                return BadRequest(Utilities.CreateResponse(message: "category not deleted", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse(message: "Deleted sucessfully", errs: null, data: "")); ;
            ;
        }

        /// <summary>
        /// Updating Category using Category Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryToUpdateDto"></param>
        /// <returns></returns>
        //https://<base_url>/api/v1/Category/update-cartegory/{CategoryId}
        [Authorize(Roles ="Admin")]
        [HttpPut("update-category/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(string categoryId, [FromBody] CategoryToUpdateDto categoryToUpdateDto)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                ModelState.AddModelError("Id", "Id is not provided");
                return BadRequest(Utilities.CreateResponse("Id must be provided", ModelState, ""));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(Utilities.CreateResponse(message: "Model state error", errs: ModelState, data: ""));
            }

            var toUpdate = await _categoryService.GetCategoryByIdAsync(categoryId);

            // check if Id is not correct
            if (toUpdate == null)
            {
                ModelState.AddModelError("Id", "Id does not exist");
                return NotFound(Utilities.CreateResponse("No record found", ModelState, ""));
            }

            // update category
            toUpdate.Name = categoryToUpdateDto.CategoryName;
            var categoryResponse = await _categoryService.UpdateCategoryAsync(toUpdate);

            if (!categoryResponse)
            {
                ModelState.AddModelError("Category", "Could not update category");
                return BadRequest(Utilities.CreateResponse(message: "category not updated", errs: ModelState, data: ""));
            }

            return Ok(Utilities.CreateResponse(message: "Updated sucessfully", errs: null, data: "")); 
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {


            var categories = await _categoryService.GetCategoriesAsync();
            var categoriesToReturn = new List<CategoryToReturnDto>();

            foreach (var item in categories)
            {
                var itemsToBeAdded = new CategoryToReturnDto
                {
                    Id = item.Id,
                    Name = item.Name
                };
                categoriesToReturn.Add(itemsToBeAdded);
            }


            return Ok(Utilities.CreateResponse(message: "All categories", errs: null, data: categoriesToReturn));
        }

        [HttpGet("nonempty-categories")]
        public async Task<IActionResult> GetNonEmptyCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var categoriesToReturn = new List<CategoryToReturnDto>();

            foreach (var item in categories)
            {
                if(item.Livestocks.Count > 0)
                {
                    var itemsToBeAdded = new CategoryToReturnDto
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    categoriesToReturn.Add(itemsToBeAdded);
                }
            }
            return Ok(Utilities.CreateResponse(message: "All nonempty categories", errs: null, data: categoriesToReturn));
        }
    }
}