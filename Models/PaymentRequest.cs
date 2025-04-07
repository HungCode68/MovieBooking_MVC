namespace MovieBookingMVC.Models
{
    public class PaymentRequest
    {
        public int ShowtimeId { get; set; }
        public List<int> SelectedSeats { get; set; }
        public decimal Amount { get; set; }  // Tổng tiền
        public decimal TotalPrice { get; set; }  // Tổng tiền

        public string PaymentMethod { get; set; } // Phương thức thanh toán (Thêm dòng này)
    }
}
