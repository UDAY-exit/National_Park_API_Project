using System;
using System.ComponentModel.DataAnnotations;

namespace NationalPark_API.Models.DTOs
{
    public class BookingCreateDTO
    {
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
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}