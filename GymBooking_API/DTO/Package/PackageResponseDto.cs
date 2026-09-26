using GymBooking_API.DTO.GYM;
using GymBooking_API.DTO.Membership;
using GymBooking_API.Entity;
using Microsoft.EntityFrameworkCore.Storage;

namespace GymBooking_API.DTO.Packages
{
    public sealed record PackageResponseDto
    {
        public int Id { get; init; }
        public required string Name { get; init; } = string.Empty;
        public required string Description { get; init; } = string.Empty;
        public required decimal DailyPrice { get; init; }  
        public required decimal MonthlyPrice { get; init; }
        public required Includes Includes { get; init; }
        public ICollection<GymShortResponseDto> GymClasses { get; init; } = new List<GymShortResponseDto>();
        public ICollection<MembershipForPackageAdminDto> Memberships { get; init; } = new List<MembershipForPackageAdminDto>();
    }
}
