using GymBooking_API.DTO.GYM;
using GymBooking_API.Entity;

namespace GymBooking_API.DTO.Coach
{
    //Քանի որ GymResponseDto - ի մեջ մենք ուզում ենք Coach - ի տվյալները ցույց տալ, բայց Coach-ի մեջ նորից Gym - երը չբերել։
    public record class CoachShortResponseDto
    {
        public required string FullName { get; init; } = string.Empty;
        public required int Age { get; init; }
        public required string Gender { get; init; } = string.Empty;
        public required string Phone { get; init; } = string.Empty;
        public required string Certification { get; init; } = string.Empty;
    }
}
