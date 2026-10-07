using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Controllers
{
    [Route("api/NationalPark")]
    [ApiController]
    public class NationalParkController : Controller
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
        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();//400
            var nationalPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalPark)){
                ModelState.AddModelError("", "Unable to update National Park!!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();//204
        }
        [HttpDelete]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
                return NotFound();
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound();
            if (!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Unable to delete National Park!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}