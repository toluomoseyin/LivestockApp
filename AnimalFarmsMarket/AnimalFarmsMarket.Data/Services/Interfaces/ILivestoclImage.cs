using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface ILivestoclImage
    {
        public Task<IEnumerable<LivestockImages>> GetLivestockImagesByLivestock(string LivestockId);
    }
}
