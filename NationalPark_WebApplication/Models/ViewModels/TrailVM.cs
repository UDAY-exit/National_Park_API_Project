using Microsoft.AspNetCore.Mvc.Rendering;

namespace NationalPark_WebApplication.Models.ViewModels
{
    public class TrailVM
    {
        public IEnumerable <SelectListItem>NationalParkList{ get; set; }
        public Trail Trail { get; set; }
    }
}
