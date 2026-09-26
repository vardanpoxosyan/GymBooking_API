namespace GymBooking_API.DTO.Coach
{
    public sealed record CoachCreateDto
    {
        public required string FullName { get; init; } = string.Empty;
        public required int Age { get;init; }
        public required string Gender { get; init; } = string.Empty;
        public required string Phone { get; init; } = string.Empty;
        public required string Certification { get; init; } = string.Empty;
    }
}
