using System.ComponentModel.DataAnnotations.Schema;
namespace MovieBookingMVC.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string MembershipLevel { get; set; } = "Basic";
        public decimal TotalSpent { get; set; } = 0;
        public int CGVPoints { get; set; } = 0;
        public string GiftCards { get; set; }
        public string Vouchers { get; set; }
        public string Coupons { get; set; }
        public string MemberCards { get; set; }

        // Liên kết với User
        [NotMapped]
        public virtual UserViewModel User { get; set; }
    }

}
