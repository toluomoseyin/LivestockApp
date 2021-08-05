using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface ITrackingService
    {
        IQueryable<TrackingHistory> GetAllTrackingHistories();
        Task<TrackingHistory> GetTrackingHistoryByIdAsync(string Id);
        Task<ICollection<TrackingHistory>> GetTrackingHistoriesByOrderAsync(string orderId, int page, int perPage);
        Task<ICollection<TrackingHistory>> GetTrackingHistoriesByDeliveryPersonAsync(string deliveryPersonId, int page, int perPage);
        Task<bool> DeleteTrackingHistoryAsync(string Id);
        Task<ICollection<TrackingHistory>> GetTrackingHistoriesByOrderAsync(string OrderId);
        Task<IEnumerable<TrackingHistory>> GetTrackingHistoriesAsync(int page, int perPage);
        int GetCount();
        Task<bool> AddTrackingHistoryAsync(TrackingHistory trackingHistory);
        Task<bool> UpdateTrackingHistoryAsync(TrackingHistory trackingHistory);
        Task<IEnumerable<TrackingHistory>> GetTrackingHistoriesByTrackingNumberAsync(string trackingNumber, string userId);
    }
}
