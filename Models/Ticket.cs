namespace MovieBookingMVC.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Price { get; set; }

        // Thuộc tính hiển thị dữ liệu liên quan
        public string UserName { get; set; }
        public string MovieTitle { get; set; }
        public string SeatNumber { get; set; }
        public string ShowTime { get; set; }
    }
}
