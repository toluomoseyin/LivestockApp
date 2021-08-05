using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Enum;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data.Services.Implementations
{
    public class DeliveryAssignmentService:IDeliveryAssignmentService
    {

        private readonly AppDbContext _cxt;
        public  int TotalCount { get; set; }

        public DeliveryAssignmentService(AppDbContext context)
        {
            _cxt = context;
        }

        private async Task<bool> SavedAsync()
        {
            var valueToReturned = false;
            if (await _cxt.SaveChangesAsync() > 0)
                valueToReturned = true;
            else
                valueToReturned = false;

            return valueToReturned;
        }
        public async Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssignmentsByStatusAsync(int page, int perPage,byte status)
        {
            var deliveryAssignment = _cxt.DeliveryAssignments.Where(x => x.Status == status);
                                                
            TotalCount = deliveryAssignment.Count();
            var assignments = await GetShapedAssignmentsResultAysnc(page, perPage, deliveryAssignment);
           
            return assignments;
        }

        public async Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssignmentsByOrderAsync(int page, int perPage, string orderId)
        {
            var deliveryAssignment = _cxt.DeliveryAssignments.Where(x => x.OrderId == orderId);
                                              
            TotalCount = deliveryAssignment.Count();
            var assignments = await GetShapedAssignmentsResultAysnc(page, perPage, deliveryAssignment);

            return assignments;
        }

        public async Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssignmentsByDeliveryPersonAsync(int page, int perPage, string deliveryPersonId)
        {
            var deliveryAssignment = _cxt.DeliveryAssignments.Where(x => x.DeliveryPersonId == deliveryPersonId);
                                              
            TotalCount = deliveryAssignment.Count();
            var assignments = await GetShapedAssignmentsResultAysnc(page, perPage, deliveryAssignment);

            return assignments;
        }



        public async Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetShapedAssignmentsResultAysnc(int page, int perPage,IQueryable<DeliveryAssignment> assignments )
        {

            var result =  assignments.Select(x=>new ShapedListOfDeliveryAssignment
            { 
                Id=x.Id,
                Status=((DeliveryAssignmentStatus)x.Status).ToString(),
                DeliveryPersonId=x.DeliveryPersonId,
                OrderId=x.OrderId,
                TrackNumber = x.Order.TrackingNumber
                
            });

            var query =await result.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            return query;
        }

       public int GetTotalCount()
        {
            return TotalCount;
        }


        public async Task<DeliveryAssignment> GetAssignmentByIdAsync(string id)
        {
            return await _cxt.DeliveryAssignments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeclineAssignmentById(DeliveryAssignment deliveryAssignment)
        {
            _cxt.DeliveryAssignments.Update(deliveryAssignment);
            return await SavedAsync();
        }

        public Task<IEnumerable<ShapedListOfDeliveryAssignment>> DeliveryAssignments(int page, int perPage)
        {
            var results = _cxt.DeliveryAssignments;
            TotalCount = results.Count();
            var paginatedResult = GetShapedAssignmentsResultAysnc(page, perPage, results);
            return paginatedResult;
        }


        public async Task<bool> AddAssignmentAsync(DeliveryAssignment assignment)
        {
            await _cxt.DeliveryAssignments.AddAsync(assignment);

            return await SavedAsync();
        }

        public async Task<bool> UpdateAssignmentAsync(DeliveryAssignment assignment)
        {
            _cxt.DeliveryAssignments.Update(assignment);

            return await SavedAsync();
        }

        public async Task<bool> DeleteAssignmentAsync(DeliveryAssignment assignment)
        {
            _cxt.DeliveryAssignments.Remove(assignment);

            return await SavedAsync();
        }

        public async Task<IEnumerable<ShapedListOfDeliveryAssignment>> GetAssgnmentByUserAndStatus(int page, int perPage, string AppUserId, byte status)
        {
            IQueryable<DeliveryAssignment> deliveryAssignment;

            if (status == 1)
            {
                deliveryAssignment = _cxt.DeliveryAssignments.Include(x => x.DeliveryPerson).Where(x => x.DeliveryPerson.AppUserId == AppUserId && x.Status == status);
            }
            else
            {
                var deliveryPerson = await _cxt.DeliveryPersons.FirstOrDefaultAsync(x => x.AppUserId == AppUserId);
                deliveryAssignment = _cxt.DeliveryAssignments.Include(x => x.Order).Where(x => x.Order.ShippedTo == deliveryPerson.CoverageLocation && x.Status == status);
            }


            TotalCount = deliveryAssignment.Count();
            var assignments = await GetShapedAssignmentsResultAysnc(page, perPage, deliveryAssignment);

            return assignments;
        }


    }
}
