namespace MovieBookingMVC.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int ShowtimeId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatType { get; set; } // Standard hoặc VIP
        public bool IsBooked { get; set; }
        
    }

}
