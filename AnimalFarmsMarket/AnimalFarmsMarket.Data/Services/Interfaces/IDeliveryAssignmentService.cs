using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Interfaces
{
    public interface IDeliveryAssignmentService
    {
        Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssignmentsByStatusAsync(int page, int perPage, byte status);
        Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssignmentsByOrderAsync(int page, int perPage, string orderId);
        Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssignmentsByDeliveryPersonAsync(int page, int perPage, string deliveryPersonId);
        Task<bool> DeclineAssignmentById(DeliveryAssignment deliveryAssignment);
        Task<IEnumerable<ShapedListOfDeliveryAssignment>> DeliveryAssignments(int page, int perPage);
        Task<DeliveryAssignment> GetAssignmentByIdAsync(string id);
        int GetTotalCount();
        Task<bool> AddAssignmentAsync(DeliveryAssignment assignment);
        Task<bool> UpdateAssignmentAsync(DeliveryAssignment assignment);
        Task<bool> DeleteAssignmentAsync(DeliveryAssignment assignment);
        Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssgnmentByUserAndStatus(int page, int perPage, string AppUserId, byte status);
    }
}
