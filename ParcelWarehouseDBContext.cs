using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport.Areas.Identity.Data;
using ParcelWarehouseManagementSupport.Models;

namespace ParcelWarehouseManagementSupport
{
    public class ParcelWarehouseDBContext : DbContext
    {
        public ParcelWarehouseDBContext(DbContextOptions<ParcelWarehouseDBContext> options) :
            base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<Address> Adresses { get; set; } 
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<Sender> Senders { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
