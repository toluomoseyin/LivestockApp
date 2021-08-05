using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implementations
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public int TotalCount;
        public OrderItemsService(AppDbContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<ICollection<OrderItems>> GetOrderItemsByOrderAsync(string OrderId)
        {
            var res = await _dbContext.OrderItems
                 .Include(x => x.Livestock)
                 .Where(x => x.OrderId == OrderId)
                 .ToListAsync();

            return res;
        }

        public async Task<OrderItems> GetOrderItemByLivestockAsync(string id)
        {
            return await _dbContext.OrderItems.Include(m => m.Livestock)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<OrderItems>> GetListOfOrdersAsync(int page, int perPage)
        {
            return await _dbContext.OrderItems.Include(x => x.Livestock).Include(o => o.Order).ToListAsync();
        }



        public IEnumerable<IGrouping<string, OrderItems>> GetUserOrders(string userid, int page, int perPage)
        {
            var orders = _dbContext.OrderItems.Include(l => l.Livestock)
                                            .Include(o => o.Order).ThenInclude(s => s.ShippingPlan)
                                            .Include(o => o.Order).ThenInclude(p => p.PaymentMethod)
                                            .Include(o => o.Order).ThenInclude(d => d.DeliveryMode)
                                            .Include(o => o.Order).ThenInclude(u => u.User).ThenInclude(a => a.Address)
                                            .Where(o => o.Order.UserId == userid)
                                            .AsEnumerable()
                                            .GroupBy(o => o.OrderId);

            TotalCount = orders.Count();
            var orderItemspaginated = orders.Skip((page - 1) * perPage).Take(perPage);

            return orderItemspaginated;

            

        }

        public int GetCount()
        {
            return TotalCount;
        }


    }
}
