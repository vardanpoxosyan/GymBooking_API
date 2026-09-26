using GymBooking_API.DTO.ApplicationUser;
using GymBooking_API.DTO.Package;
using GymBooking_API.Entity;

namespace GymBooking_API.DTO.Membership
{
    // Ամրագրումենի համար ստեղծված class որը նախատեսվախ է Admin ի համար սովորական User Պետք չի տեսնի սրանք
    public sealed record class MembershipAdminResponseDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int PackageId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public MembershipType Type { get; init; }
        public decimal TotalAmount { get; init; }
        public PackageShortResponseDto? Package { get; init; }
        public ResponseForBookingaAndMembershipDTO? User { get; init; }
    }
}
