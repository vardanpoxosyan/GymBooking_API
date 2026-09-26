using GymBooking_API.Entity;
using System.Reflection;

namespace GymBooking_API.DTO.Membership
{
    public sealed record MembershipUpdateDto
    {
        public required int PackageId { get; init; }
        public required MembershipType Type { get; init; }
    }
}
