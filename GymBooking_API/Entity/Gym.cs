namespace GymBooking_API.Entity
{
        public class Gym
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            //Ով է մարզչը
            public int CoachId { get; set; }
            public Coach Coach { get; set; } = null!;
            //Սա ասում է Կոնկրետ դասը քանի ամրագրում ունի
            public ICollection<Booking> Bookings { get; set; }= new List<Booking>();
           //Որ փաթեթներում է այս դասը ներառված
           public ICollection<Package> Packages { get; set; }= new List<Package>();
    }
}
