using MovieBookingMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Showtime
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public string CinemaRoom { get; set; } // Thay vì CinemaName
    public int CinemaId { get; set; }
    public DateTime ShowDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime Showtimes { get; set; }

    public decimal Price { get; set; }

    public virtual Movies Movie { get; set; }  // Navigation property
    public virtual Cinema Cinema { get; set; } // Navigation property

   
}
