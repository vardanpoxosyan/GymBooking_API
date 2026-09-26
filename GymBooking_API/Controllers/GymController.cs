using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.DTO.GYM;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymController(ApplicationDbContext context,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var classes = await context.Gyms
                .Include(x => x.Coach)
                .Include(x => x.Packages)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var result = mapper.Map<IEnumerable<GymResponseDto>>(classes);
            return Ok(result);
        }
        // GET: api/Gym/1
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(
                    "Id must be greater than zero.");
            }

            var gymClass = await context.Gyms
                .Include(x => x.Coach)
                .Include(x => x.Packages)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

            if (gymClass is null)
            {
                return NotFound(
                    $"Gym with Id {id} was not found.");
            }

            var result = mapper.Map<GymResponseDto>(gymClass);

            return Ok(result);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(GymCreateDto dto,CancellationToken cancellationToken)
        {
            var gymClass = mapper.Map<Gym>(dto);
            if (dto.PackageIds.Count > 0)
            {
                var packages = await context.Packages.Where(x => dto.PackageIds.Contains(x.Id)).ToListAsync(cancellationToken);

                if (packages.Count != dto.PackageIds.Count)
                {
                    return BadRequest(
                        "One or more Package IDs were not found.");
                }
                foreach (var package in packages)
                {
                    gymClass.Packages.Add(package);
                }
            }
            await context.Gyms.AddAsync(gymClass,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            var result = mapper.Map<GymResponseDto>(gymClass);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(int id,GymUpdateDto dto,CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(
                    "Id must be greater than zero.");
            }

            var gymClass = await context.Gyms
                .Include(x => x.Packages)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

            if (gymClass is null)
            {
                return NotFound(
                    $"Gym with Id {id} was not found.");
            }

            mapper.Map(dto, gymClass);

            // Հին Package-ները մաքրում ենք
            gymClass.Packages.Clear();

            // Նոր Package-ները բերում ենք DB-ից
            if (dto.PackageIds.Count > 0)
            {
                var packages = await context.Packages
                    .Where(x => dto.PackageIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
                if (packages.Count != dto.PackageIds.Count)
                {
                    return BadRequest(
                        "One or more Package IDs were not found.");
                }
                foreach (var package in packages)
                {
                    gymClass.Packages.Add(package);
                }
            }
            await context.SaveChangesAsync(cancellationToken);
            var result = mapper.Map<GymResponseDto>(gymClass);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(
                    "Id must be greater than zero.");
            }

            var gymClass = await context.Gyms
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

            if (gymClass is null)
            {
                return NotFound(
                    $"Gym with Id {id} was not found.");
            }

            context.Gyms.Remove(gymClass);

            await context.SaveChangesAsync(
                cancellationToken);

            return Ok(
                $"Gym with Id {id} was successfully deleted.");
        }
    }
}