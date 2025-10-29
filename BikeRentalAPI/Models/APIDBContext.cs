using Microsoft.EntityFrameworkCore;

namespace BikeRentalAPI.Models
{
    public class APIDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalStatus> RentalStatuses { get; set; }
        public APIDBContext(DbContextOptions<APIDBContext> options)
           : base(options) { }
    }
}
