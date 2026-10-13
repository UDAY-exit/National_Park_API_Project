namespace NationalPark_WebApplication.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParksList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }
    }
}
