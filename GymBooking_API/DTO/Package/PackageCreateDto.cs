using GymBooking_API.Entity;

namespace GymBooking_API.DTO.Packages
{
    public sealed record PackageCreateDto
    {
        public required string Name { get; init; } = string.Empty;
        public required string Description { get; init; } = string.Empty;
        public required decimal DailyPrice { get; init; }
        public required decimal MonthlyPrice { get; init; }
        public required Includes Includes { get; init; }
    }
}
