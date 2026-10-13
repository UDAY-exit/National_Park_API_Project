using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalPark_WebApplication.Models;
using NationalPark_WebApplication.Models.ViewModels;
using NationalPark_WebApplication.Repository;
using NationalPark_WebApplication.Repository.IRepository;


namespace NationalPark_WebApplication.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailController(ITrailRepository trailRepository,INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkApiPath);
            TrailVM trailVM = new TrailVM()
            {
                NationalParkList = nationalParkList.Select(np => new SelectListItem()
                {
                    Text = np.Name,
                    Value = np.Id.ToString()
                }),
                Trail = new Trail()
            };
            if (id == null) return View(trailVM);
            trailVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath,id.Value);
            if (trailVM.Trail == null) return NotFound();
            return View(trailVM);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if (trailVM == null) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(trailVM);
            }
            if (trailVM.Trail.Id == 0)
            {
                await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
            }
            else
            {
                await _trailRepository.UpdateAsync(SD.TrailAPIPath, trailVM.Trail);
            }
              
                return RedirectToAction("Index");
            
        }


        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data =await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (!status)
                return Json(new { success = false, message = "Unable to delete data !!!" });
            return Json(new { success = true, message = " Data is Deleated succesfully!!!" });

        }
        #endregion
    }
}