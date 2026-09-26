namespace GymBooking_API.DTO.ApplicationUser
{
    public sealed record RegisterDto
    {
        public required string FullName { get; init; }

        public required string Email { get; init; }

        public required string Password { get; init; }
    }
}
