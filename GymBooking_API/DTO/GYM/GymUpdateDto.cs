namespace GymBooking_API.DTO.GYM
{
    public sealed record GymUpdateDto
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
        public required DateTime Start { get; init; }
        public required DateTime End { get; init; }
        public required int CoachId { get; init; }
        public List<int> PackageIds { get; init; } = new();
    }
}
