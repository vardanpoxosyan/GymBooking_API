using AutoMapper;
using GymBooking_API.Data;
using GymBooking_API.DTO.Cart;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Security.Claims;

namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ApplicationDbContext _context,IMapper mapper) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCarts()
        {
            //Login Եղած User ից վերցնում Id 
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            //Գնտում ենք Տվյալ User-ի cart-ը

            var cart = await _context.Carts.
                Include(s => s.CartItems)
                .ThenInclude(s => s.Package).FirstOrDefaultAsync(s => s.UserId == userId);
            if (cart==null)
            {
                return NotFound("Cart not found");
            }
            var response = mapper.Map<CartResponseDto>(cart);

            return Ok(response);    
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            // 1. Ստանում ենք login եղած User-ի Id-ն
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
         
            // 2. Գտնում ենք Package-ը
            var package = await _context.Packages .FirstOrDefaultAsync(p => p.Id == dto.PackageId);
            if (package == null)
            {
                return NotFound("Package not found");
            }
            // 3. Գտնում ենք User-ի Cart-ը
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
          
            // 4. Եթե Cart չունի՝ ստեղծում ենք
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
       
            // 5. Ստուգում ենք՝ Package-ը արդեն Cart-ում կա՞
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(
                    c =>c.CartId == cart.Id && c.PackagId == dto.PackageId);
        
            // 6. Եթե կա՝ ավելացնում ենք Quantity-ը
            if (cartItem != null)
            {
                cartItem.Quantity += dto.Quantity;
            }
            // 7. Եթե չկա՝ ստեղծում ենք նոր CartItem
            else
            {
                cartItem = new Entity.CartItem
                {
                    CartId = cart.Id,
                    PackagId = dto.PackageId,
                    Quantity = dto.Quantity
                };
                _context.CartItems.Add(cartItem);
            }
            await _context.SaveChangesAsync();
            return Ok("Package added to cart");
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> CartUpdate(int cartitemid,UpdateCartItemDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cartitem = await _context.CartItems.Include(s => s.Cart).
                FirstOrDefaultAsync(s => s.CartId == cartitemid && s.Cart.UserId == userId);

            if (cartitem ==null)
            {
                return NotFound("Cart item not found");
            }
            if (dto.Quantity<=0)
            {
                return BadRequest("Quanity must be greater than 0");
            }
            cartitem.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            return Ok("Quantity updated seccessfully");
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveCartItem(int cartitemId)
        {
            if (cartitemId <= 0)
            {
                return BadRequest("Id must be greater than zero");
            }

            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var cartItem = await _context.CartItems
                .Include(s => s.Cart)
                .SingleOrDefaultAsync(s =>
                    s.Id == cartitemId &&
                    s.Cart.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok("Cart item removed successfully");
        }
    }
}
