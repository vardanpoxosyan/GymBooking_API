namespace GymBooking_API.DTO.GYM
{
    public sealed record GymCreateDto
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
        public required DateTime Start { get; init; }
        public required DateTime End { get; init; }

        //Սրանով կապում ենք արգդեն գոյություն ունեցող Coach-ի հետ
        public required int CoachId { get; init; }
        // Որ Packages-ներում պետք է լինի այս Gym-ը
        public ICollection<int> PackageIds { get; init; }
            = new List<int>();
    }
}
