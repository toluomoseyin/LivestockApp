using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _ctx;

        public ReviewService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public int TotalCount { get; set; }

        public async Task<IEnumerable<Review>> GetReviewsByLivestock(string livestockId)
        {
            return await _ctx.Reviews.Where(x => x.LivestockId == livestockId).ToListAsync();
        }
        
        public async Task<AppUser> GetReviewerOnALivestock(string livestockId, string userId)
        {
            var res = await _ctx.Reviews.Include(s => s.User).Where(x => x.LivestockId == livestockId && x.UserId == userId).Select(s => s.User).SingleOrDefaultAsync();
            return res;
        }

        public async Task<Review> GetReviewAsync(string id)
        {
            return await _ctx.Reviews.FindAsync(id);
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            await _ctx.Reviews.AddAsync(review);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Review>> GetReviewsByLiveStockAsync(string livestockId, int page, int perPage)
        {
            var reviews = _ctx.Reviews
                .Where(review => review.LivestockId == livestockId)
                .OrderByDescending(review => review.DateCreated);

            TotalCount = await reviews.CountAsync();
            
            var paginateReviews = await Paginate(reviews, page, perPage);
            
            return paginateReviews;
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId, int page, int perPage)
        {
            var reviews = _ctx.Reviews
                .Where(review => review.UserId == userId)
                .OrderByDescending(review => review.DateCreated);
            
            TotalCount = await reviews.CountAsync();

            var paginateReviews = await Paginate(reviews, page, perPage);

            return paginateReviews;
        }

        public async Task<Review> CheckReviewByUserAsync(string userId, string livestockId)
        {
            return await _ctx.Reviews.Where(x => x.UserId == userId && x.LivestockId == livestockId).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _ctx.Reviews.Update(review);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReview(string id)
        {
            var review = await _ctx.Reviews.FindAsync(id);
            if (review == null) return false;
            
            _ctx.Reviews.Remove(review);
            return await _ctx.SaveChangesAsync() > 0;
        }
        
        private static async Task<IEnumerable<Review>> Paginate(IQueryable<Review> reviews, int page, int perPage)
        {
            return await reviews.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }
    }
}
