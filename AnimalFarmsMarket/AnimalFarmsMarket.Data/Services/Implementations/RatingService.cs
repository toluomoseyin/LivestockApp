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
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _ctx;
        public int totalCount { get; set; }
        public RatingService(AppDbContext ctx)
        {
            _ctx = ctx;
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

        public async Task<IEnumerable<Rating>> pagenate(IQueryable<Rating> ratings, int page, int perPage)
        {
            var result = await ratings.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return result;

        }


        public async Task<Rating> GetRatingByIdAsync(string ratingId)
        {
            return await _ctx.Ratings.FirstOrDefaultAsync(rating => rating.Id == ratingId);
        }

        public async Task<bool> AddRatingAsync(Rating rating)
        {
            await _ctx.Ratings.AddAsync(rating);
            return await SavedAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByLivestockAsync(string LivestockId, int page , int perPage)
        {
            var ratingsfromDB = _ctx.Ratings.Where(x => x.LivestockId == LivestockId);
            totalCount = ratingsfromDB.Count();
            var pagnatedResult = await pagenate(ratingsfromDB, page, perPage);
            return pagnatedResult;
        }

        public async Task<IEnumerable<Rating>> GetRatingsByUserAsync(string UserId, int page, int perPage)
        {
            var ratingsfromDB = _ctx.Ratings.Where(x => x.UserId == UserId);
            totalCount = ratingsfromDB.Count();
            var pagnatedResult = await pagenate(ratingsfromDB, page, perPage);
            return pagnatedResult;
        }

        public async Task<Rating> CheckRatingByUserAsync(string userId, string livestockId)
        {
            return await _ctx.Ratings.Where(x => x.UserId == userId && x.LivestockId == livestockId).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateRatingAsync(Rating rating)
        {
            _ctx.Ratings.Update(rating);
            return await SavedAsync();

        }

        public async Task<bool> DeleteRatingAsync(string ratingId)
        {
            var ratingToDelete = await GetRatingByIdAsync(ratingId);

            if (ratingToDelete == null)
            {
                return false;
            }
            _ctx.Ratings.Remove(ratingToDelete);
            return await SavedAsync();
        }



        public async Task<IEnumerable<Rating>> GetRatingsByLivestock(string LivestockId)
        {
            return await _ctx.Ratings.Where(x => x.LivestockId == LivestockId).ToListAsync();
        }

        public async Task<decimal> GetRating(string LivestockId)
        {
            decimal rates = 0;

            var rating = await _ctx.Ratings.Where(rating => rating.LivestockId == LivestockId).ToListAsync();
            var count = rating.Count();
            foreach (var rate in rating)
            {
                rates += rate.RatingFigure;
            }
            if (count < 2)
                return Math.Round(rates, 1);

            return Math.Round((rates / count), 1);
        }

        public int GetCount()
        {
            return totalCount;
        }
    }
}
