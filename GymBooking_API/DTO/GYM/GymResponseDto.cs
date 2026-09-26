using GymBooking_API.DTO.Coach;
using GymBooking_API.DTO.Package;
using GymBooking_API.DTO.Packages;

namespace GymBooking_API.DTO.GYM
{

    //Ընդհանուր Gym-ի տվյալները
    public sealed record GymResponseDto
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        //Սա ցույց է տալիս այց մարզիչի Id-in որը տվյալ պարապունքը անցկացնում է
        public int CoachId { get; init; }
        public CoachShortResponseDto Coach { get; init; } = null!;
        //Տվյալ Gym-ի փաթեթները այսինքն թե ես դասը քանի փաթեթում կա
        public ICollection<PackageShortResponseDto> Packages { get; init; }= new List<PackageShortResponseDto>();
    }
}
