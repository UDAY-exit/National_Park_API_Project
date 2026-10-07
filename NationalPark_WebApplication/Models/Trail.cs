using System.ComponentModel.DataAnnotations;

namespace NationalPark_WebApplication.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Distance { get; set; }
        [Required]
        public string Elevation { get; set; }
        public enum DIfficultyType  {Easy,Moderate,Difficult }
        [Required]
        public DIfficultyType DIfficulty { get; set; }
        [Display(Name = "National Park")]
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; }
    }
}
