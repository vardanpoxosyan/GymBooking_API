using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.DTO.Coach;
using GymBooking_API.DTO.GYM;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachController(ApplicationDbContext _context,IMapper _mapper) : ControllerBase
    {
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var coaches = await _context.Coaches.Include(s=>s.GymClasses).AsNoTracking().ToListAsync(cancellationToken);
            var result = _mapper.Map<IEnumerable<CoachResponseDto>>(coaches);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            if (id<=0)
            {
                return BadRequest("Id must be greater then zero");
            }
            var coach = await _context.Coaches.
                Include(s=>s.GymClasses).AsNoTracking().SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

            if (coach is null)
            {
                return NotFound($"Coach with Id {id} was not found.");
            }
            var result = _mapper.Map<CoachResponseDto>(coach);

            return Ok(result);
        }
        [HttpPost("CreateCoach")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(CoachCreateDto coachCreateDto,CancellationToken cancellationToken)
        {
            if (coachCreateDto is null)
            {
                return BadRequest("Coach can't be null");
            }
            var coach = _mapper.Map<Coach>(coachCreateDto);
            await _context.Coaches.AddAsync(coach, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var result = _mapper.Map<CoachResponseDto>(coach);
            return Ok(result);
        }
        [HttpPut("UpdateCoachDetail")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id, CoachUpdateDto coachupdatedto, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest($"Id must be greater than zero.");
            }
            if (coachupdatedto is null)
            {
                return BadRequest($"Coach can't be null.");
            }

            var coach = await _context.Coaches.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (coach is null)
            {
                return NotFound($"Coach with Id {id} was not found.");
            }
            _mapper.Map(coachupdatedto, coach);
            await _context.SaveChangesAsync(cancellationToken);
            var result = _mapper.Map<CoachResponseDto>(coach);
            return Ok(result);
        }
        [HttpDelete("DeleteCoach")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest($"Id must be greater than zero.");
            }
            var coach = await _context.Coaches.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (coach is null)
            {
                return NotFound($"Coach with Id {id} was not found.");
            }

            _context.Coaches.Remove(coach);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok("Coach Deleted Succefully");
        }
    }
}
