namespace GymBooking_API.DTO.ApplicationUser
{
    public sealed record ResponseForBookingaAndMembershipDTO
    {
        public required string FullName { get; init; }
        public required string Email { get; init; }

    }
}
