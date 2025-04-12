using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreVisitTracking.Domain.Entities;
using System.Reflection;

namespace StoreVisitTracking.Infrastructure
{
    public class StoreVisitTrackingDbContext : DbContext
    {
        protected IConfiguration _configuration { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public StoreVisitTrackingDbContext(DbContextOptions<StoreVisitTrackingDbContext> options, IConfiguration configuration)
               : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
        }
    }
}
