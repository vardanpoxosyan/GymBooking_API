using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymBooking_API.DTO.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(
         ApplicationDbContext _context,
         IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            // 1. Ստանում ենք login եղած User-ի Id-ն
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            // 2. Գտնում ենք միայն այդ User-ի Orders-ը
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Package)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            // 3. Map դեպի Response DTO
            var response = mapper.Map<IEnumerable<OrderResponseDto>>(orders);
            return Ok(response);
        }
        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            // 1. Ստանում ենք login եղած User-ի Id-ն
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            // 2. Գտնում ենք User-ի Cart-ը
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Package)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            // 3. Cart չկա
            if (cart == null)
            {
                return NotFound("Cart not found");
            }
            // 4. Cart-ը դատարկ է
            if (!cart.CartItems.Any())
            {
                return Conflict("Cart is empty");
            }
            // 5. Ստեղծում ենք Order
            var order = new Entity.Order
            {
                UserId = userId
            };
            // 6. CartItem-ները դարձնում ենք OrderItem
            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    PackageId = cartItem.PackagId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Package.MonthlyPrice
                };
                order.OrderItems.Add(orderItem);
            }
            // 7. Հաշվում ենք Order-ի TotalAmount-ը
            order.TotalAmount = order.OrderItems.Sum(
                item => item.UnitPrice * item.Quantity);
            
            // 8. Պահում ենք Order-ը
            _context.Orders.Add(order);
            // 9. Մաքրում ենք Cart-ը
            _context.CartItems.RemoveRange(cart.CartItems);
            
            // 10. Պահպանում ենք Database-ում
            await _context.SaveChangesAsync();
         
            // 11. Նորից բերում ենք Order-ը Package-ներով
            var createdOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Package)
                .FirstAsync(o => o.Id == order.Id);
            // 12. Map դեպի Response DTO
            var response = mapper.Map<OrderResponseDto>(createdOrder);

            return Ok(response);
        }
    }
}
