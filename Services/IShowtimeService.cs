using MovieBookingMVC.ViewModels;

namespace MovieBookingMVC.Services
{
    public interface IShowtimeService
    {
        ShowtimeViewModel GetShowtimeDetails(int showtimeId);
    }
}
