namespace GymBooking_API.DTO.Booking
{
    public sealed record BookingCreateDto
    {
        public required int GymClassId { get; init; }
        public DateTime BookedAt { get; init; }= DateTime.UtcNow;
    }
}
