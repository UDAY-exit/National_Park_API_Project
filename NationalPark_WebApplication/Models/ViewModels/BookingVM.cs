
using System;
using System.ComponentModel.DataAnnotations;

namespace NationalPark_WebApplication.Models.ViewModels
{
    public class BookingVM
    {
        public int NationalParkId { get; set; }

        public string NationalParkName { get; set; } = string.Empty;

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
    }
}
