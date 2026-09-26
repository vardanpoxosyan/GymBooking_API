using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.DTO.Package;
using GymBooking_API.DTO.Packages;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController(ApplicationDbContext _context,IMapper _mapper) : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var packages = await _context.Packages.Include(s=>s.GymClasses).AsNoTracking().ToListAsync(cancellationToken);
            var result = _mapper.Map<IEnumerable<PackageResponseDto>>(packages);
            return Ok(result);
        }
        [HttpGet("GetPackageById/{id}")]
        public async Task<IActionResult> GetByID(int id, CancellationToken cancellationToken)
        {
            var package = await _context.Packages.Include(s => s.GymClasses)
                .AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (package is null)
            {
                return NotFound($"Package with Id {id} was not found.");
            }
            var result = _mapper.Map<PackageResponseDto>(package);
            return Ok(result);
        }
        [HttpGet("PackagesAndMamberShipsOnlyAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMembershipAdmin(CancellationToken cancellationToken)
        {
            var packages = await _context.Packages
                .Include(s => s.GymClasses).Include(s=>s.Memberships).ThenInclude(s=>s.User).AsNoTracking().ToListAsync(cancellationToken);
            var result = _mapper.Map<IEnumerable<PackageResponseDto>>(packages);
            return Ok(result);
        }
        [HttpGet("GetPackagesIncludes")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetIncludes()
        {
            var includes = Enum.GetValues<Includes>()
                .Select(x => new
                {
                    Id = (int)x,
                    Name = x.ToString()
                });
            return Ok(includes);
        }
        [HttpPost("CreatePackages")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(PackageCreateDto package,CancellationToken cancellationToken)
        {
            if (package is null)
            {
                return BadRequest($"Package can't be null.");
            }
            var createpakage = _mapper.Map<Package>(package);
            await _context.Packages.AddAsync(createpakage,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var result = _mapper.Map<PackageResponseDto>(createpakage);
            return Ok(result);  
        }
        [HttpPut("UpdatePackages")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,PackageUpdateDto packagedto,CancellationToken cancellationToken)
        {
            if (id<=0)
            {
                return BadRequest($"Id must be greater than zero.");
            }
            if (packagedto is null)
            {
                return BadRequest($"Package can't be null.");
            }
            var package = await _context.Packages.SingleOrDefaultAsync(s => s.Id == id,cancellationToken);
            if (package is null)
            {
                return NotFound($"Package with Id {id} was not found.");
            }
            _mapper.Map(packagedto, package);
            await _context.SaveChangesAsync(cancellationToken);
            var result = _mapper.Map<PackageResponseDto>(package);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            if (id<=0)
            {
                return BadRequest($"Id must be greater than zero.");
            }
            var delate=await _context.Packages.SingleOrDefaultAsync(s=>s.Id== id,cancellationToken);
            if (delate is null)
            {
                return NotFound($"Package with Id {id} was not found.");
            }
             _context.Packages.Remove(delate);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok("Package Deleted Succefully");
        }
    }
}
