using MovieBookingMVC.Models;
namespace MovieBookingMVC.ViewModels
{
    public class QuickBookingViewModel
    {
        public string SelectedDate { get; set; }
        public int SelectedCinemaId { get; set; } // Đổi từ SelectedRoomId thành SelectedCinemaId
        public List<Movies> Movies { get; set; } = new List<Movies>();
        public List<Cinema> Cinemas { get; set; } = new List<Cinema>(); // Đổi từ Rooms -> Cinemas
    }
}
