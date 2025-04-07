namespace MovieBookingMVC.ViewModels
{
    public class CheckoutViewModel
    {
        public int ShowtimeId { get; set; }
        public string MovieTitle { get; set; }
        public string CinemaRoom { get; set; }
        public DateTime Showtime { get; set; }
        public List<string> SeatNumbers { get; set; }
        public decimal TotalPrice { get; set; }
        public List<int> SelectedSeats { get; set; }
    }
}
