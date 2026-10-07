using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Controllers
{
    [Route("api/Trail")]
    [ApiController]
     public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().Select(_mapper.Map<TrailDto>));
        }

        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);

            if (trail == null)
                return NotFound();

            var trailDto = _mapper.Map<Trail, TrailDto>(trail);

            if (trailDto == null)
                return NotFound();

            return Ok(trailDto);
        }

        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            if (trailDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail in Db !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var trail = _mapper.Map<TrailDto, Trail>(trailDto);

            if (!_trailRepository.createTrail(trail))
            {
                ModelState.AddModelError("", "Unable To Create Trail!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtRoute(
                "GetTrail",
                new { trailId = trail.Id },
                trail
            );
        }

        [HttpPut]
        public IActionResult UpdateTrail([FromBody] TrailDto trailDto)
        {
            if (trailDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var trail = _mapper.Map<TrailDto, Trail>(trailDto);

            if (!_trailRepository.updateTrail(trail))
            {
                ModelState.AddModelError("", "Unable To Update Trail!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
                return NotFound();

            var trail = _trailRepository.GetTrail(trailId);

            if (trail == null)
                return NotFound();

            if (!_trailRepository.deleteTrail(trail))
            {
                ModelState.AddModelError("", "Unable To Delete Trail!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}