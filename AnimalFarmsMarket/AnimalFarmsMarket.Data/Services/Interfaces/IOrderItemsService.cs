using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IOrderItemsService
    {
        Task<ICollection<OrderItems>> GetOrderItemsByOrderAsync(string OrderId);

        Task<OrderItems> GetOrderItemByLivestockAsync(string id);

        Task<IEnumerable<OrderItems>> GetListOfOrdersAsync(int page, int perPage);

        IEnumerable<IGrouping<string, OrderItems>> GetUserOrders(string userid, int page, int perPage);

        int GetCount();
    }
}
