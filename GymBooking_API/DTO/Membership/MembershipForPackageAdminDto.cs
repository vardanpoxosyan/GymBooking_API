using GymBooking_API.DTO.ApplicationUser;
using GymBooking_API.Entity;

namespace GymBooking_API.DTO.Membership
{
    public sealed record MembershipForPackageAdminDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public MembershipType Type { get; init; }
        public decimal TotalAmount { get; init; }
        public ResponseForBookingaAndMembershipDTO? User { get; init; }
    }
}
