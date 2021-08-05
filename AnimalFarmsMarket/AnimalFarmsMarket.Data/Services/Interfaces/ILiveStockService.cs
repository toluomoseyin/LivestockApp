using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface ILiveStockService
    {
        public int TotalCount { get; set; }
        Task<IEnumerable<ShappedListOfLivestock>> GetLiveStocksAndMainImageAsync(int page, int perPage);
        Task<Livestock> GetLivestockByIdAsync(string Id);
        Task<Livestock> GetLivestockFullDetailsByIdAsync(string Id);
        Task<bool> AddLivestockImageAsync(LivestockImages livestock);
        Task<bool> DeleteLivestockImageAsync(string livestockId, string publicId);
        Task<LivestockImages> GetLivestockImageAsync(string livestockId, string publicId);
        Task<LivestockImages> GetLivestockMainImage(string livestockId);
        Task<IEnumerable<LivestockImages>> GetLivestockImages(string livestockId);
        Task<bool> DeleteAllLivestockImagesAsync(string livestockId);
        Task<IEnumerable<ShappedListOfLivestock>> GetLivestocksByLocationAsync(int page, int perPage, string location);
        Task<IEnumerable<ShappedListOfLivestock>> GetLivestocksByLocationAndMarketAsync(int page, int perPage,
            string location, string market);
        Task<IEnumerable<ShappedListOfLivestock>> GetLivestocksByQueriesAsync(SearchLivestockDto query, int perPage);
        Task<bool> DeleteLivestockAsync(Livestock livestock);
        Task<bool> AddLivestockAsync(Livestock livestock);
        Task<bool> UpdateLivestock(Livestock livestock);
        IEnumerable<LocationMarketDto> GetLiveStockLocation();
        
        IEnumerable<IEnumerable<string>> GetBreedsSexesAndWeightsForLivestocksCategory(string category);
        Task<IEnumerable<ShappedListOfLivestock>> GetLivestockByUserIdAsync(string userId, int page, int perPage);

        Task<IEnumerable<Market>> GetMarketsAsync();

        Task<IEnumerable<Livestock>> GetAgentLivestocks(string agentid);
        Task<IEnumerable<IGrouping<string, Livestock>>> GetAgentLivestocksAndMarkets(string agentid);
        Task<IEnumerable<ShappedListOfLivestock>> GetLiveStocksAndMainImageByAgentAsync(int page, int perPage, string userId);
    }
}
