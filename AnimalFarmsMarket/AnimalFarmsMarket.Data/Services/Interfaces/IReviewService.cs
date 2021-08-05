using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IReviewService
    {
        int TotalCount { get; set; }
        Task<IEnumerable<Review>> GetReviewsByLivestock(string livestockId);
        Task<AppUser> GetReviewerOnALivestock(string livestockId, string userId);
        Task<Review> GetReviewAsync(string id);
        Task<bool> AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsByLiveStockAsync(string livestockId, int page, int perPage);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId, int page, int perPage);
        Task<Review> CheckReviewByUserAsync(string userId, string livestockId);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReview(string id);
    }
}
