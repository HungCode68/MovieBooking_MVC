using Microsoft.EntityFrameworkCore; // Thêm thư viện này nếu chưa có
using MovieBookingMVC.Data;
using MovieBookingMVC.ViewModels;
using System.Linq;

namespace MovieBookingMVC.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly ApplicationDbContext _context;

        public ShowtimeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ShowtimeViewModel GetShowtimeDetails(int showtimeId)
        {
            var showtime = _context.Showtimes
                .Include(s => s.Movie)  // Include Movie để tránh lỗi null
                .Include(s => s.Cinema) // Include Cinema để tránh lỗi null
                .Where(s => s.Id == showtimeId)
                .Select(s => new ShowtimeViewModel
                {
                    Id = s.Id,
                    MovieTitle = s.Movie != null ? s.Movie.Title : "N/A",
                    ShowDate = s.ShowDate.ToString("yyyy-MM-dd"),
                    Times = new List<string> { s.StartTime.ToString("HH:mm") },
                    CinemaRoom = s.Cinema != null ? s.Cinema.Name : "N/A",
                    Price = s.Price
                })
                .FirstOrDefault();

            return showtime;
        }
    }
}
