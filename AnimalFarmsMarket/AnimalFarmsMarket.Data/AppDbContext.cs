using AnimalFarmsMarket.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Data
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Agent> Agents { get; set; }

        public DbSet<Address> Addresses { get; set; }

       public DbSet<TrackingHistory> TrackingHistories { get; set; }

        internal Task<string> FindAsync(string id)
        {
            throw new NotImplementedException();
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Testimony> Testimonies { get; set; }

        public DbSet<DeliveryPerson> DeliveryPersons { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Livestock> LiveStocks { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<LivestockImages> LivestockImages { get; set; }

        public DbSet<Market> Markets { get; set; }

        public DbSet<MarketAddress> MarketAddresses { get; set; }

        public DbSet<BroadcastNews> BroadCastNews { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<OrderItems> OrderItems { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<DeliveryMode> DeliveryModes { get; set; }

        public DbSet<ShippingPlan> ShippingPlans { get; set; }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; }
    }
}
