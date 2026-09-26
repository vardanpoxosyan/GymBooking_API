using GymBooking_API.DTO.ApplicationUser;
using GymBooking_API.DTO.Package;

namespace GymBooking_API.DTO.Membership
{
    public sealed record MembershipResponseDto
    {
        public required int Id { get; init; }
        public required int UserId { get; init; }
        public required int PackageId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public required decimal TotalAmount { get; init; }
        public required PackageShortResponseDto PackageShortResponseDto { get; init; }    
    }
}
