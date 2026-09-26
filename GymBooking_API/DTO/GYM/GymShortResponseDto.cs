namespace GymBooking_API.DTO.GYM
{
    public sealed record GymShortResponseDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Location { get; init; }
    }
}
