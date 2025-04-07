using MovieBookingMVC.Data;
using MovieBookingMVC.Models;

public class SeatService : ISeatService
{
    private readonly ApplicationDbContext _context;

    public SeatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<List<Seat>> GetSeatsByShowtime(int showtimeId)
    {
        var seats = _context.Seats
            .Where(s => s.ShowtimeId == showtimeId)
            .OrderBy(s => s.SeatNumber)
            .ToList();

        int cols = 10; // Giả sử mỗi hàng có 10 ghế
        var seatLayout = new List<List<Seat>>();

        for (int i = 0; i < seats.Count; i += cols)
        {
            seatLayout.Add(seats.Skip(i).Take(cols).ToList());
        }

        return seatLayout;
    }
    public List<string> GetSeatNumbersByIds(List<int> seatIds)
    {
        return _context.Seats
            .Where(s => seatIds.Contains(s.Id))
            .Select(s => s.SeatNumber)
            .ToList();
    }
}
