namespace GymBooking_API.DTO.Booking
{
    public sealed record BookingUpdateDto
    {
        public required int UserId { get; init; }
        public required int GymClassId { get; init; }
        public DateTime BookedAt { get; init; }
    }
}
