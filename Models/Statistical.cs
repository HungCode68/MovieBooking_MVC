namespace MovieBookingMVC.Models
{
    public class Statistical
    {
        public int TotalUsers { get; set; }
        public int TotalMovies { get; set; }
        public int TotalShowtimes { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalBookings { get; set; }
        public int TotalSeats { get; set; }
        public int TotalSeatsBooked { get; set; }
        public int TotalPayments { get; set; }
        public DateTime LastUpdated { get; set; }

        // Thêm danh sách doanh thu theo phim
        public List<MovieRevenue> RevenueByMovie { get; set; } = new List<MovieRevenue>();
        public MovieRevenue HighestRevenueMovie { get; set; }
    }
    public class MovieRevenue
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
