using GymBooking_API.Entity;

namespace GymBooking_API.DTO.Membership
{
    public sealed record class MembershipCreateDto
    {
        public required int PackageId { get; init; }
        public required MembershipType Type { get; init; }
    }
}
