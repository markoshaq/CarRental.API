using CarRental.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Data
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<RentalContract> RentalContracts{ get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
