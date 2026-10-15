using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NationalPark_API.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        [ForeignKey("NationalParkId")]
        public NationalPark NationalPark { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime BookingDate { get; set; }

        [Range(0, 5)]
        public int Adults { get; set; }

        [Range(0, 5)]
        public int Children { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}

