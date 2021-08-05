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
    public class LivestockImageService : ILivestoclImage
    {
        private readonly AppDbContext _ctx;
        public LivestockImageService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<LivestockImages>> GetLivestockImagesByLivestock(string LivestockId)
        {
            return await _ctx.LivestockImages.Where(l => l.LivestockId == LivestockId).ToListAsync();
        }
    }
}
