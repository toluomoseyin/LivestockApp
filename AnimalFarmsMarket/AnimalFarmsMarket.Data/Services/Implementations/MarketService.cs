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
    public class MarketService : IMarketService
    {
        private readonly AppDbContext _dbcontext;

        public MarketService(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<DeliveryMode>> GetDeliveryModes()
        {
            var response = await _dbcontext.DeliveryModes.ToListAsync();
            return response;
        }

        public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
        {
            var response = await _dbcontext.PaymentMethods.ToListAsync();
            return response;
        }

        public async Task<IEnumerable<ShippingPlan>> GetShippingPlans()
        {
            var response = await _dbcontext.ShippingPlans.ToListAsync();
            return response;
        }

        public async  Task<PaymentMethod> GetPaymentMethodById(string OrderId)
        {
            return await _dbcontext.PaymentMethods.Where(x => x.Id == OrderId).SingleOrDefaultAsync();
        }
    }
}
