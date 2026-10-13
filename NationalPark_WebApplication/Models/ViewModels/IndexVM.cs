namespace NationalPark_WebApplication.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }
        public IEnumerable<Trail> Trails { get; set; }
    }
}
