using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository nationalParkRepository,
            IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkDtoList = _nationalParkRepository.GetNationalParks().Select(_mapper.Map<NationalParkDto>);
            return Ok(nationalParkDtoList);
        }
        [HttpGet("{nationalParkId:int}",Name ="GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId) {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            var nationalParkDto = _mapper.Map<NationalPark, NationalParkDto>(nationalPark);
            if (nationalParkDto == null) return NotFound();
            return Ok(nationalParkDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var nationalPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (_nationalParkRepository.NationalParkExists(nationalPark.Name))
            {
                ModelState.AddModelError("", "National Park in db!!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Unable to create National Park !!!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id }, nationalPark);
        }
    }
}