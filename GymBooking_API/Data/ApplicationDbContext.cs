using GymBooking_API.Entity;
using Microsoft.EntityFrameworkCore;

namespace GymBooking_API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions): DbContext(dbContextOptions)
    {
        public DbSet<Package> Packages { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Coach> Coaches { get; set; }   
        public DbSet<Cart>Carts { get; set; }   
        public DbSet<CartItem> CartItems{ get; set; }   
        public DbSet<Order> Orders{ get; set; }   
        public DbSet<OrderItem>OrderItems{ get; set; }   
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Membership=>User-One-to-Many
            modelBuilder.Entity<Membership>().HasOne(m => m.User).WithMany(u => u.Memberships)
                 .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
            //Coach=>Gym One-to Many
            modelBuilder.Entity<Gym>().HasOne(g => g.Coach).WithMany(c => c.GymClasses)
                .HasForeignKey(g => g.CoachId).OnDelete(DeleteBehavior.Restrict);//Եվ եթե User-ը ջնջվի => նրա Membership-ները նույնպես կջնջվեն։
            //Booking=>User One to Many
            modelBuilder.Entity<Booking>().HasOne(b => b.User).WithMany(u => u.Bookings)
                 .HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);//Այսինքն Gym-ը ջնջելու դեպքում կապված Booking-ները նույնպես կջնջվեն։

            //Booking=>Gym  One to many
            modelBuilder.Entity<Booking>().HasOne(b => b.GymClass).WithMany(g => g.Bookings)
                 .HasForeignKey(b => b.GymClassId).OnDelete(DeleteBehavior.Cascade);//Cascade-եթե ջնջում ենք Parant -ը նրա հետ ջնջվում են ավտոմատ Child Entity ները

            //Gym=>Package many-to many
            modelBuilder.Entity<Gym>().HasMany(s => s.Packages).WithMany(s => s.GymClasses);

            modelBuilder.Entity<Membership>()
                 .Property(x => x.TotalAmount).HasPrecision(18, 2);

            modelBuilder.Entity<Package>()
                .Property(x => x.DailyPrice).HasPrecision(18, 2);

            modelBuilder.Entity<Package>()
                .Property(x => x.MonthlyPrice).HasPrecision(18, 2);

            modelBuilder.Entity<Package>().Property(s => s.Includes).HasConversion<string>();
            modelBuilder.Entity<Gym>().Property(s => s.Start).HasConversion<string>();
            modelBuilder.Entity<Gym>().Property(s => s.End).HasConversion<string>();

            modelBuilder.Entity<Membership>().Property(s => s.Type).HasConversion<string>();

            //Cart=>CartItem one to many
            modelBuilder.Entity<CartItem>().HasOne(s => s.Cart)
                .WithMany(s => s.CartItems).HasForeignKey(s=>s.CartId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>().HasOne(s => s.Package)
                .WithMany(s => s.CartItems).HasForeignKey(s=>s.PackagId).OnDelete(DeleteBehavior.Restrict);

            //Order
            modelBuilder.Entity<Order>()
                        .HasOne(o => o.User)
                        .WithMany()
                        .HasForeignKey(o => o.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                        .HasOne(oi => oi.Order)
                        .WithMany(o => o.OrderItems)
                        .HasForeignKey(oi => oi.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
