using Microsoft.AspNetCore.Mvc;
using NationalPark_WebApplication.Models;
using NationalPark_WebApplication.Repository.IRepository;

namespace NationalPark_WebApplication.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null) return View(nationalPark);
            nationalPark = await _nationalParkRepository.GetAsync(SD.NationalParkApiPath, id.GetValueOrDefault());
            if (nationalPark == null) return NotFound();
            return View(nationalPark);
        }

        [HttpPost]
        public async Task<IActionResult>Upsert(NationalPark nationalPark)
        {
            if (nationalPark == null) return NotFound();
            if (!ModelState.IsValid) return View(nationalPark);
            if (nationalPark.Id == 0)
                await _nationalParkRepository.CreateAsync(SD.NationalParkApiPath, nationalPark);
            else
                await _nationalParkRepository.UpdateAsync(SD.NationalParkApiPath, nationalPark);
            return RedirectToAction(nameof(Index));
        }


        #region APIs

        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _nationalParkRepository
                .GetAllAsync(SD.NationalParkApiPath) });
        }

        #endregion
    }
}
