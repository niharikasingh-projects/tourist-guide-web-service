using Microsoft.EntityFrameworkCore;
using TouristGuide.API.Models;

namespace TouristGuide.API.Data
{
    public class TouristGuideDbContext : DbContext
    {
        public TouristGuideDbContext(DbContextOptions<TouristGuideDbContext> options)
            : base(options)
        {
        }

        public DbSet<TouristAttraction> TouristAttractions { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TouristAttraction>()
                .Property(t => t.Rating)
                .HasPrecision(3, 1);

            modelBuilder.Entity<Guide>()
                .Property(g => g.HourlyRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Guide>()
                .Property(g => g.Rating)
                .HasPrecision(3, 1);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Amount)
                .HasPrecision(10, 2);
        }
    }
}
