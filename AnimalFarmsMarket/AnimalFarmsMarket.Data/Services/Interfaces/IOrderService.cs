using AnimalFarmsMarket.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IOrderService
    {
        public int TotalCount { get; set; }
        public Task<Order> GetOrderByIdAsync(string OrderId);
        public Task<Order> GetOrderByTrackingIdAsync(string TrackingId);
        public Task<IEnumerable<Order>> GetOrderByConfirmationStatusAsync(byte status, int pageNumber, int perPage);
        Task<bool> AddOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Order order);
        public Task<IEnumerable<Order>> GetOrdersByPaymentStatusAsync(byte PaymentStatus, int page, int perPage);
        public Task<IEnumerable<Order>> GetOrderByLiveStockAsync(int page, int perPage, string LiveStockId);
        public Task<IEnumerable<Order>> GetOrdersByStatusAsync(byte status, int page, int perPage);
        Task<Order> CalculateOrderPrice(Order order);
        public Task<Order> FindOrderByIdAsync(string orderId);

        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string id, int page, int perPage);
        Task<IEnumerable<Order>> GetOrdersByAdminAsync(int page, int perPage);

        Task<IEnumerable<Order>> GetOrders(int page, int perPage);
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(string userId, int page, int perPage);
        Task<IEnumerable<Order>> GetOrdersByLoggedInUserAsync(string userId, int page, int perPage);
        Task<bool> UpdateOrderByIdAsync(Order model);
        Task<IEnumerable<ShapedListOfOrderItem>> GetOrderByAgentIdAsync(string userId, int page, int perPage);

    }
}
