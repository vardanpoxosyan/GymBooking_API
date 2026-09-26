using GymBooking_API.Entity;

namespace GymBooking_API.DTO.Package
{
    public sealed record PackageShortResponseDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public decimal DailyPrice { get; init; }
        public decimal MonthlyPrice { get; init; }
        public Includes Includes { get; init; }
    }
}
