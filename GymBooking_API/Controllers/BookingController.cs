using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.DTO.Booking;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(ApplicationDbContext context,IMapper mapper) : ControllerBase
    {
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var bookings = await context.Bookings.Include(s=>s.User).Include(s=>s.GymClass).AsNoTracking().ToListAsync(cancellationToken);

            var result = mapper.Map<IEnumerable<BookingResponseDto>>(bookings);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var booking = await context.Bookings.Include(s=>s.GymClass).Include(s=>s.User)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
            if (booking is null)
            {
                return NotFound($"Booking with Id {id} was not found.");
            }
            var result = mapper.Map<BookingResponseDto>(booking);
            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(BookingCreateDto dto,CancellationToken cancellationToken)
        {
            // JWT-ից վերցնում ենք ներկայիս User-ի Id-ն
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // 1. Ստուգում ենք՝ GymClass գոյություն ունի՞
            var gymClassExists = await context.Gyms
                .AnyAsync(
                    x => x.Id == dto.GymClassId,
                    cancellationToken);

            if (!gymClassExists)
            {
                return NotFound(
                    $"GymClass with Id {dto.GymClassId} was not found.");
            }

            // 2. Ստուգում ենք՝ User-ը ունի՞
            // ակտիվ Membership, որի Package-ը ներառում է այս GymClass-ը
            var hasAccessToGym = await context.Memberships
                .AnyAsync(
                    s =>
                        s.UserId == userId &&
                        s.StartDate <= DateTime.UtcNow &&
                        s.EndDate >= DateTime.UtcNow &&
                        s.Package.GymClasses.Any(
                            g => g.Id == dto.GymClassId),
                    cancellationToken);

            if (!hasAccessToGym)
            {
                return BadRequest(
                    "You don't have an active membership for this gym.");
            }

            // 3. Ստուգում ենք՝ User-ը արդեն Booking արե՞լ է
            var alreadyBooked = await context.Bookings
                .AnyAsync(
                    s =>
                        s.UserId == userId &&
                        s.GymClassId == dto.GymClassId,
                    cancellationToken);

            if (alreadyBooked)
            {
                return BadRequest(
                    "You have already booked this class.");
            }

            // 4. DTO → Booking
            var booking = mapper.Map<Booking>(dto);

            // UserId-ը վերցնում ենք JWT-ից,
            // ոչ թե client-ից
            booking.UserId = userId;

            await context.Bookings.AddAsync(
                booking,
                cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            var createdBooking = await context.Bookings.Include(x => x.User).Include(x => x.GymClass).FirstAsync(x => x.Id == booking.Id,cancellationToken);

            var result = mapper.Map<BookingResponseDto>(createdBooking);

            return Ok(result);
        }
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id,BookingUpdateDto dto,CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var booking = await context.Bookings
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId,cancellationToken);
            if (booking is null)
            {
                return NotFound($"Booking with Id {id} was not found.");
            }
            var gymClassExists = await context.Gyms
                .AnyAsync(x => x.Id == dto.GymClassId,cancellationToken);

            if (!gymClassExists)
            {
                return NotFound(
                    $"GymClass with Id {dto.GymClassId} was not found.");
            }
            mapper.Map(dto, booking);

            await context.SaveChangesAsync(cancellationToken);

            var result = mapper.Map<BookingResponseDto>(booking);

            return Ok(result);
        }


        // DELETE: api/Booking/1
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var booking = await context.Bookings
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId,cancellationToken);

            if (booking is null)
            {
                return NotFound(
                    $"Booking with Id {id} was not found.");
            }

            context.Bookings.Remove(booking);

            await context.SaveChangesAsync(cancellationToken);

            return Ok(
                $"Booking with Id {id} was successfully deleted.");
        }
    }
}
