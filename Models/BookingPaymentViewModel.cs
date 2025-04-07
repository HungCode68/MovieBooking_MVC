namespace MovieBookingMVC.Models
{
    public class BookingPaymentViewModel
    {
        public int BookingId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
        public string CinemaName { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime StartTime { get; set; }
        public string SeatNumbers { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime BookingTime { get; set; }
        public string UserFullName { get; set; }
        public int UserId { get; set; }

    }
}
