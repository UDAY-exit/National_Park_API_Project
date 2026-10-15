using System;
using System.ComponentModel.DataAnnotations;

namespace NationalPark_WebApplication.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
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
        public string NationalParkName { get; set; } = string.Empty;
        public string? NationalParkDescription { get; set; }
        public string? NationalParkLocation { get; set; }
        public string? NationalParkImageUrl { get; set; }
    }
}