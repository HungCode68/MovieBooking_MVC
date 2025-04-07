namespace MovieBookingMVC.ViewModels
{
    public class ShowtimeViewModel
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string ShowDate { get; set; } // Định dạng yyyy-MM-dd cho frontend dễ xử lý
        public List<string> Times { get; set; } = new List<string>(); // Danh sách giờ chiếu
        public string CinemaRoom { get; set; } // Thay vì CinemaName
        public DateTime Showtime { get; set; }
        public DateTime Showtimes { get; set; }

        public decimal Price { get; set; }
    }
}
