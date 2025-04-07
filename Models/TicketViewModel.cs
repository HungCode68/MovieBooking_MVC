namespace MovieBookingMVC.Models
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
        public string CinemaName { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime StartTime { get; set; } // Đổi kiểu dữ liệu cho phù hợp với SQL
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public DateTime BookingDate { get; set; }
    }


}
