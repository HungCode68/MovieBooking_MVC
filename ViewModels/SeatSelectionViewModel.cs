using MovieBookingMVC.Models;

namespace MovieBookingMVC.ViewModels
{
    public class SeatSelectionViewModel
    {
        public int ShowtimeId { get; set; }
        public string MovieTitle { get; set; }   // Thêm thuộc tính này
        public string CinemaRoom { get; set; }       // Thêm thuộc tính này
        public DateTime Showtime { get; set; }   // Thêm thuộc tính này

        public decimal Price { get; set; }  // Thêm thuộc tính này
        public List<List<Seat>> SeatLayout { get; set; } = new List<List<Seat>>();
    }
}
