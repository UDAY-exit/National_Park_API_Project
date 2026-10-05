using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private INationalParkRepository _nationalParkRepository;
        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
    }
}
