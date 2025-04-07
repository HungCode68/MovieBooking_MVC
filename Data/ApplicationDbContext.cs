using Microsoft.EntityFrameworkCore;
using MovieBookingMVC.Models; // Import các model của bạn
using MovieBookingMVC.ViewModels;

namespace MovieBookingMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Seat> Seats { get; set; } // Thêm DbSet phù hợp
        public DbSet<Showtime> Showtimes { get; set; } // Thêm DbSet phù hợp
        public DbSet<Cinema> Cinemas { get; set; } // Thêm DbSet phù hợp
        public DbSet<Movies> Movies { get; set; } // Thêm DbSet phù hợp
        public DbSet<UserDetails> UserDetails { get; set; } // Thêm DbSet phù hợp
        public DbSet<User> Users { get; set; } // Thêm DbSet phù hợp
        public DbSet<Ticket> Tickets { get; set; } // Thêm DbSet phù hợp

        public DbSet<ShowtimeViewModel> ShowtimeViewModels { get; set; } // Thêm DbSet phù hợp





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserViewModel>().HasNoKey(); // 👈 Thêm dòng này
        }

    }

}
