using BCrypt.Net;
using GymBooking_API.Data;
using GymBooking_API.DTO.ApplicationUser;
using GymBooking_API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace GymBooking_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext applicationDbContext,IConfiguration configuration) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto,CancellationToken cancellationToken)
        {
            //Ստուգում ենք Email գոյություն ունի թե ոչ
            var emailexist = await applicationDbContext.ApplicationUsers
                .AnyAsync(s => s.Email == registerDto.Email,cancellationToken);
            if (emailexist)
            {
                return Conflict("This Email is already registered");
            }
            //Password Hash ենք անում
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            //Ստեղծում ենք User
            var user = new ApplicationUser
            {
                FullName=registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash=passwordHash,
                Role="User"
            };
            //Ավելացնում ենք Database
            await applicationDbContext.ApplicationUsers.AddAsync(user, cancellationToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            return Ok("Registered Successfully");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto,CancellationToken cancellationToken)
        {
            //Գտնում ենք User Email - ով
            var user = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(s => s.Email == loginDto.Email);
            if (user is null)
            {
                return Unauthorized("Invalid Email or Password");
            }
            //ստուգում ենք Password
            var validpassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!validpassword)
            {
                return Unauthorized("Invalid Email or Password");
            }
            //Ինֆորմացիա ենք փոխանցում JWT ին Claim ով
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.Role,user.Role),
            };
            //Jwt Setting
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtSettings["Key"]!));
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
            // 5. Ստեղծում ենք JWT
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(
                        jwtSettings["ExpirationMinutes"]!)),
                signingCredentials: credentials);

            // 6. JWT → string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenString);
        }
    }
}
