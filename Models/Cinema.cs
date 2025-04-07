using System.ComponentModel.DataAnnotations;

namespace MovieBookingMVC.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string NumberRoom { get; set; }
    }
}
