using GymBooking_API.DTO.ApplicationUser;
using GymBooking_API.DTO.GYM;

namespace GymBooking_API.DTO.Booking
{
    public sealed record BookingResponseDto
    {
        public required int Id { get; init; }
        public required int UserId { get; init; }
        public required int GymClassId { get; init; }
        public DateTime BookedAt { get; init; }
        public required GymShortResponseDto Gym { get; init; }
        public required ResponseForBookingaAndMembershipDTO UserDetail { get; init; }
    }
}
