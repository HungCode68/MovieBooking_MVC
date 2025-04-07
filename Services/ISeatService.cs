using MovieBookingMVC.Models;

public interface ISeatService
{
    List<List<Seat>> GetSeatsByShowtime(int showtimeId);
    List<string> GetSeatNumbersByIds(List<int> seatIds);
}
