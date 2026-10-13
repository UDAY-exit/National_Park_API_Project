using Microsoft.AspNetCore.Mvc;
using NationalPark_WebApplication.Models;
using NationalPark_WebApplication.Models.ViewModels;
using NationalPark_WebApplication.Repository.IRepository;
using System.Diagnostics;

namespace NationalPark_WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;
        public HomeController(INationalParkRepository nationalParkRepository,
            ITrailRepository trailRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }
        public async Task<IActionResult> Index()
        {
            IndexVM indexVM = new IndexVM()
            {
                NationalParksList = await _nationalParkRepository.GetAllAsync(SD.NationalParkApiPath),
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath)
            };
            return View(indexVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
