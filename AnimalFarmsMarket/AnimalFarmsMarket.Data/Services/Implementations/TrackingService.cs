using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implementations
{
    public class TrackingService : ITrackingService
    {
        public int TotalCount { get; set; }

        private readonly AppDbContext _dbContext;
        public TrackingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<bool> SavedAsync()
        {
            var valueToReturned = false;
            if (await _dbContext.SaveChangesAsync() > 0)
                valueToReturned = true;
            else
                valueToReturned = false;

            return valueToReturned;
        }

        public IQueryable<TrackingHistory> GetAllTrackingHistories()
        {
            var allTrackingHistories = _dbContext.TrackingHistories;
            return allTrackingHistories;
        }


        public async Task<ICollection<TrackingHistory>> GetTrackingHistoriesByOrderAsync(string OrderId)
        {
            var histories = await _dbContext.TrackingHistories.Where(x => x.OrderId == OrderId).ToListAsync();

            return histories;
        }

        public async Task<TrackingHistory> GetTrackingHistoryByIdAsync(string Id)
        {
            return await _dbContext.TrackingHistories.FindAsync(Id);
        }

        public async Task<ICollection<TrackingHistory>> GetTrackingHistoriesByOrderAsync(string orderId, int page, int perPage)
        {
            var histories = _dbContext.TrackingHistories.Where(x => x.OrderId == orderId);

            TotalCount = histories.Count();
            var trackingsPaginated = await histories.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return trackingsPaginated;
        }

        public async Task<bool> DeleteTrackingHistoryAsync(string Id)
        {
            var taskToDelete = await GetTrackingHistoryByIdAsync(Id);

            _dbContext.TrackingHistories.Remove(taskToDelete);
            return await SavedAsync();
        }

        public async Task<ICollection<TrackingHistory>> GetTrackingHistoriesByDeliveryPersonAsync(string deliveryPersonId, int page, int perPage)
        {

            var histories = _dbContext.TrackingHistories.Where(x => x.DeliveryPersonId == deliveryPersonId);

            TotalCount = histories.Count();
            var historiesPaginated = await histories.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return historiesPaginated;
        }
        public async Task<bool> AddTrackingHistoryAsync(TrackingHistory trackingHistory)
        {
            await _dbContext.TrackingHistories.AddAsync(trackingHistory);
            return await SavedAsync();
        }
        public async Task<bool> UpdateTrackingHistoryAsync(TrackingHistory trackingHistory)
        {
            _dbContext.TrackingHistories.Update(trackingHistory);
            return await SavedAsync();
        }

        public async Task<IEnumerable<TrackingHistory>> GetTrackingHistoriesAsync(int page, int perPage)
        {
            var allTrackingHistories = GetAllTrackingHistories();
            var paginatedTracking = await allTrackingHistories.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return paginatedTracking;
        }

        public async Task<IEnumerable<TrackingHistory>> GetTrackingHistoriesByTrackingNumberAsync(string trackingNumber, string userId)
        {
            var trackings = await _dbContext.TrackingHistories.Include(x => x.Order).ThenInclude(x => x.User).Include(x => x.DeliveryPerson).ThenInclude(x => x.AppUser)
                                        .Where(x => x.Order.TrackingNumber == trackingNumber.ToString()
                                         && x.OrderId == x.Order.Id
                                         && x.DeliveryPerson.Id == x.DeliveryPersonId).ToListAsync();
            return trackings;
        }
        public int GetCount()
        {
            var allTrackingHistories = GetAllTrackingHistories();
            TotalCount = allTrackingHistories.Count();
            return TotalCount;
        }
    }
}
