using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface ICategoryService
    {
        public int TotalCount { get; set; }

        public Task<IEnumerable<Category>> GetCategoriesAsync(int pageNumber, int perPage);

        public Task<IEnumerable<Category>> GetCategoriesAsync();

        public Task<Category> GetCategoryByIdAsync(string categoryId);

        public int GetCount();

        public Task<bool> AddCategoryAsync(Category category);

        public Task<bool> UpdateCategoryAsync(Category category);

        public Task<bool> DeleteCategoryAsync(Category category);
    }
}
