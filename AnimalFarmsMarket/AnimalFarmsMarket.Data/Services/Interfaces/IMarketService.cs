using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IMarketService
    {
        Task<IEnumerable<ShippingPlan>> GetShippingPlans();
        Task<IEnumerable<DeliveryMode>> GetDeliveryModes();
        Task<IEnumerable<PaymentMethod>> GetPaymentMethods();
        Task<PaymentMethod> GetPaymentMethodById(string OrderId);
    }
}
