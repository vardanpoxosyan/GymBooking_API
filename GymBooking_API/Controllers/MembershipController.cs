using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.DTO.Membership;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController(ApplicationDbContext context,IMapper mapper) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllMemberships")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var memberships = await context.Memberships.Include(s=>s.User).Include(s=>s.Package)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var result = mapper.Map<IEnumerable<MembershipAdminResponseDto>>(memberships);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var membership = await context.Memberships
                .Include(s=>s.User).Include(x => x.Package).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
            if (membership is null)
            {
                return NotFound($"Membership with Id {id} was not found.");
            }
            var result = mapper.Map<MembershipAdminResponseDto>(membership);
            return Ok(result);
        }
        [HttpPost("CreateMembership")]
        [Authorize]
        public async Task<IActionResult> Create(MembershipCreateDto dto,CancellationToken cancellationToken)    
        {
            // JWT-ից վերցնում ենք ներկայիս User-ի Id-ն
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
         
            // Ստուգում ենք Package-ը
            var package = await context.Packages.FirstOrDefaultAsync(x => x.Id == dto.PackageId,cancellationToken);
            if (package is null)
            {
                return NotFound($"\"Package with Id {dto.PackageId} was not found.\"");
            }

            //memership-ի Startdate
            var startdate = DateTime.UtcNow;

            //EndDate
            var enddate = dto.Type == MembershipType.Daily ? startdate.AddDays(1) : startdate.AddMonths(1);

            //totalAmount որոշում ենք Package-ի գնից
            var totalAmont = dto.Type == MembershipType.Daily ? package.DailyPrice : package.MonthlyPrice;

            //ստեղծում ենք Membership

            var membership = new Membership
            {
                UserId=userId,
                PackageId=package.Id,
                Type=dto.Type,
                StartDate=startdate,
                EndDate=enddate,    
                TotalAmount=totalAmont,
            };

            await context.Memberships.AddAsync(membership, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var resultMembership = await context.Memberships.Include(x => x.Package).FirstOrDefaultAsync(x => x.Id == membership.Id,cancellationToken);
            var result = mapper.Map<MembershipResponseDto>(resultMembership);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id,MembershipUpdateDto dto,CancellationToken cancellationToken)
        {
            //Jwt Վերցնում ենք UserId
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
          
            //Գտնում ենք Membership և ստոգում որ պատկանում է ներկայիս User ին
            var membership = await context.Memberships.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId,cancellationToken);
            if (membership is null)
            {
                return NotFound($"Membership with Id {id} was not found.");
            }
            //Գտնում ենք նոր Package
            var package = await context.Packages.FirstOrDefaultAsync(x => x.Id == dto.PackageId,cancellationToken);
            if (package is null)
            {
                return NotFound(
                    $"Package with Id {dto.PackageId} was not found."
                );
            }
            //Նոր Startdate
            var startDate = DateTime.UtcNow;

            //Նոր EndDate
            var endDatae = dto.Type == MembershipType.Daily ? startDate.AddDays(1) : startDate.AddMonths(1);

            //Նոր TotalAmount
            var totalAmount = dto.Type == MembershipType.Daily ? package.DailyPrice : package.MonthlyPrice;

            membership.PackageId= package.Id;
            membership.StartDate = startDate;
            membership.EndDate = endDatae;
            membership.TotalAmount = totalAmount;
            membership.Type = dto.Type;

            await context.SaveChangesAsync(cancellationToken);
          
            var resultMembership = await context.Memberships.Include(x => x.Package).FirstAsync(x => x.Id == membership.Id, cancellationToken);
            var result = mapper.Map<MembershipResponseDto>(resultMembership);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var membership = await context.Memberships.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);

            if (membership is null)
            {
                return NotFound($"Membership with Id {id} was not found.");
            }
            context.Memberships.Remove(membership);
            await context.SaveChangesAsync(cancellationToken);
            return Ok($"Membership with Id {id} was successfully deleted.");
        }
    }
}
