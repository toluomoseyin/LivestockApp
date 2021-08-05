using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _ctx;

        public int TotalCount { get; set; }

        public CategoryService(AppDbContext appDbContext)
        {
            _ctx = appDbContext;
        }

        private async Task<bool> SavedAsync()
        {
            var valueToReturned = false;
            if (await _ctx.SaveChangesAsync() > 0)
                valueToReturned = true;
            else
                valueToReturned = false;

            return valueToReturned;
        }

        private IQueryable<Category> GetCategories()
        {
            var category = _ctx.Categories.Include(l => l.Livestocks);
            TotalCount = category.Count();
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int page, int perPage)
        {
            var query = await GetCategories().Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return query;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var query = await GetCategories().ToListAsync();
            return query;
        }

        public async Task<Category> GetCategoryByIdAsync(string categoryId)
        {
            var query = await _ctx.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            return query;
        }

        public int GetCount()
        {
            TotalCount = _ctx.Categories.Count();
            return TotalCount;
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            await _ctx.Categories.AddAsync(category);
            return await SavedAsync();
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _ctx.Categories.Update(category);
            return await SavedAsync();
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            _ctx.Categories.Remove(category);
            return await SavedAsync();
        }

        public IEnumerable<Category> GetNonEmptyCategories()
        {
            var nonEmptyCategories = GetCategories().Where(c => (c.Livestocks.Count > 0)).AsEnumerable();
            return nonEmptyCategories;
        }
    }
}
