using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Models;

namespace TouristGuide.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TouristAttraction> TouristAttractions { get; set; }
        public DbSet<GuideProfile> GuideProfiles { get; set; }
        public DbSet<GuideAvailableDate> GuideAvailableDates { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasDefaultValue("tourist");
            });

            // TouristAttraction configuration
            modelBuilder.Entity<TouristAttraction>(entity =>
            {
                entity.Property(e => e.Rating).HasPrecision(3, 2);
                entity.Property(e => e.EntryFee).HasPrecision(10, 2);
            });

            // GuideProfile configuration
            modelBuilder.Entity<GuideProfile>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.AttractionId }).IsUnique();
                entity.Property(e => e.Rating).HasPrecision(3, 2);
                entity.Property(e => e.PricePerHour).HasPrecision(10, 2);

                entity.HasOne(g => g.User)
                    .WithOne(u => u.GuideProfile)
                    .HasForeignKey<GuideProfile>(g => g.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.Attraction)
                    .WithMany(a => a.GuideProfiles)
                    .HasForeignKey(g => g.AttractionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // GuideAvailableDate configuration
            modelBuilder.Entity<GuideAvailableDate>(entity =>
            {
                entity.HasOne(d => d.GuideProfile)
                    .WithMany(g => g.AvailableDates)
                    .HasForeignKey(d => d.GuideProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Booking configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
                entity.Property(e => e.TaxAmount).HasPrecision(10, 2);
                entity.Property(e => e.GrandTotal).HasPrecision(10, 2);

                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Guide)
                    .WithMany(g => g.Bookings)
                    .HasForeignKey(b => b.GuideId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Attraction)
                    .WithMany(a => a.Bookings)
                    .HasForeignKey(b => b.AttractionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Payment configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Amount).HasPrecision(10, 2);

                entity.HasOne(p => p.Booking)
                    .WithOne(b => b.Payment)
                    .HasForeignKey<Payment>(p => p.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
