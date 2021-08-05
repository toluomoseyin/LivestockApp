using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implementations
{
    public class Orderservice : IOrderService
    {
        private readonly AppDbContext _ctx;
        private readonly IOrderItemsService _orderItemsService;
        private readonly ITrackingService _trackingHistoryService;
        public int TotalCount { get; set; }

        public Orderservice(AppDbContext appDbContext, IOrderItemsService orderItemsService,
            ITrackingService trackingHistoryService)
        {
            _orderItemsService = orderItemsService;
            _trackingHistoryService = trackingHistoryService;
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

        public async Task<IEnumerable<Order>> GetOrders(int page, int perPage)
        {
            var order = _ctx.Orders;
            TotalCount = order.Count();
            var paginatedOrder = await order.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return paginatedOrder;
        }

        public async Task<IEnumerable<Order>> GetOrderByUserIdAsync(string userId, int page, int perPage)
        {
            var order = _ctx.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated);
            TotalCount = order.Count();
            var paginateOrder = await order.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return paginateOrder;
        }

        public async Task<IEnumerable<ShapedListOfOrderItem>> GetOrderByAgentIdAsync(string userId, int page, int perPage)
        {
            var orderItems = _ctx.OrderItems.Include(x => x.Livestock)
                                            .ThenInclude(x => x.Agent)
                                            .Include(x => x.Order)
                                            .Where(x => x.Livestock.Agent.AppUserId == userId)
                                            .OrderByDescending(x => x.DateCreated)
                                            .Select(x => new ShapedListOfOrderItem { Id = x.LivestockId, 
                                                                                    Amount = x.Livestock.SellingPrice * x.Quantity, 
                                                                                    Status = x.Order.Status, Quantity = x.Quantity, 
                                                                                    DateCreated = DateTime.Parse(x.DateUpdated),
                                                                                    AccountNumber = x.Livestock.Agent.AccountNumber, 
                                                                                    BankName = x.Livestock.Agent.Bank,
                                                                                    recipient = x.Livestock.Agent.Recipient,
                                                                                    OrderId = x.OrderId,
                                                                                    OrderItemId = x.Id,
                                                                                    AgentPaid = x.AgentPaid});

            TotalCount = orderItems.Count();
            var paginateOrder = await orderItems.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return paginateOrder;
        }

        public async Task<IEnumerable<Order>> GetOrderByConfirmationStatusAsync(byte status, int page, int perPage)
        {
            var order = _ctx.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Livestock)
                .Where(x => x.Status == status);
                

            TotalCount = order.Count();
            var orderpaginated = await order.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return orderpaginated;
        }
        
        public async Task<Order> GetOrderByIdAsync(string OrderId)
        {
            var order = await _ctx.Orders
                .Include(x => x.User)
                .ThenInclude(x => x.Address)
                .Include(x => x.PaymentMethod)
                .Include(x => x.ShippingPlan)
                .Include(x =>x.DeliveryMode)
                .FirstOrDefaultAsync(x => x.Id == OrderId);

            if (order == null)
            {
                return null;
            }

            order.OrderItems = await _orderItemsService.GetOrderItemsByOrderAsync(order.Id);

            return order;
        }

        public async Task<Order> GetOrderByTrackingIdAsync(string TrackingId)
        {
            var order = await _ctx.Orders
                .Include(x => x.User)
                .ThenInclude(x => x.Address)
                .Include(x => x.PaymentMethod)
                .Include(x => x.ShippingPlan)
                .Include(x => x.DeliveryMode)
               .FirstOrDefaultAsync(x => x.TrackingNumber == TrackingId);

            order.TrackingHistories = await _trackingHistoryService.GetTrackingHistoriesByOrderAsync(order.Id);

            return order;
        }

        public async Task<bool> AddOrderAsync(Order order)
        {
            var compOrder = await CalculateOrderPrice(order);
            await _ctx.Orders.AddAsync(compOrder);
            return await SavedAsync();
        }

        public async Task<Order> CalculateOrderPrice(Order order)
        {

            var items = order.OrderItems.ToDictionary(x => x.LivestockId, x => x.Quantity);

            var livestocks = await _ctx.LiveStocks.Where(x => items.Keys.Contains(x.Id))
                                                  .Select(x => new { SellingPrice = x.SellingPrice, Id = x.Id })
                                                  .ToListAsync();

            foreach (var item in livestocks)
            {
                order.PaymentAmount += item.SellingPrice * items[item.Id];
            }

            order.PaymentAmount += _ctx.ShippingPlans.Where(x => x.Id == order.ShippingPlanId).Select(x => x.DeliveryCost).SingleOrDefault();

            var user =await _ctx.Users.FindAsync(order.UserId);
            order.User = user;

            return order;
        }

        public async Task<bool> DeleteOrderAsync(Order order)
        {
            _ctx.Orders.Remove(order);
            return await SavedAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByPaymentStatusAsync(byte paymentStatus, int page, int perPage)
        {
            var orders = _ctx.Orders
                .Where(o => o.PaymentStatus == paymentStatus)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Livestock);

            TotalCount = orders.Count();
            var orderpaginated = await orders.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return orderpaginated;
        }

        public async Task<IEnumerable<Order>> GetOrderByLiveStockAsync(int page, int perPage, string LiveStockId)
        {
            var orderItems = _ctx.OrderItems
                .Include(x => x.Order)
                .ThenInclude(x => x.OrderItems)
                .ThenInclude(x => x.Livestock)
                .Where(x => x.LivestockId == LiveStockId);
            var orders = orderItems.Select(x => x.Order);

            TotalCount = orders.Count();

            var orderpaginated = await orders.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return orderpaginated;

        }

        //Get orders by status --Onas
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(byte status, int page, int perPage)
        {
            var order = _ctx.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Livestock)
                .Where(x => x.Status == status);


            TotalCount = order.Count();
            var orderpaginated = await order.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return orderpaginated;
        }

        public async Task<Order> FindOrderByIdAsync(string orderId)
        {
            var order = await _ctx.Orders.FindAsync(orderId);
            return order;
        }

        public async Task<bool> UpdateOrderByIdAsync(Order model)
        {
            _ctx.Orders.Update(model);
            return await SavedAsync();
        }

        //Get orders by customer
        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string id, int page, int perPage)
        {
            var ans = await _ctx.Orders.Where(x => x.UserId == id).ToListAsync();
            TotalCount = ans.Count;
            return await _ctx.Orders.Where(x => x.UserId == id).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        //Get orders by admin
        public async Task<IEnumerable<Order>> GetOrdersByAdminAsync(int page, int perPage)
        {
            var ans = await _ctx.Orders.Include(x  => x.User).Select(x => x).ToListAsync();
            TotalCount = ans.Count;
            return await _ctx.Orders.Select(x => x).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        //Get orders by logged in user
        public async Task<IEnumerable<Order>> GetOrdersByLoggedInUserAsync(string userId, int page, int perPage)
        {
            var ans = await _ctx.Orders.Select(x => x).ToListAsync();
            TotalCount = ans.Count;

            return await _ctx.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Livestock)
                .Where(x => x.UserId == userId).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }
    }
}
