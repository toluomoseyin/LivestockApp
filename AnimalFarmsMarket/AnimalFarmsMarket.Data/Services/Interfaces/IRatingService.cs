using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IRatingService
    {
        public Task<IEnumerable<Rating>> GetRatingsByLivestock(string LivestockId);
        public Task<Rating> GetRatingByIdAsync(string RatingId);
        public Task<bool> UpdateRatingAsync(Rating rating);
        public Task<IEnumerable<Rating>> GetRatingsByLivestockAsync(string LivestockId, int perPage, int page);
        public Task<IEnumerable<Rating>> GetRatingsByUserAsync(string LivestockId, int perPage, int page);
        public Task<Rating> CheckRatingByUserAsync(string userId, string livestockId);
        public Task<IEnumerable<Rating>> pagenate(IQueryable<Rating> ratings, int perPage, int page);
        public Task<bool> DeleteRatingAsync(string ratingId);
        public Task<decimal> GetRating(string LivestockId);
        public Task<bool> AddRatingAsync(Rating rating);
        public int GetCount();
    }
}
