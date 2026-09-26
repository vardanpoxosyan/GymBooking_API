namespace GymBooking_API.Entity
{
    public class Coach
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; }=string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Certification { get; set; } = string.Empty;
        //մի մարզիչը կարող է ունենաշ շատ դասեր
        public ICollection<Gym> GymClasses { get; set; }= new List<Gym>();
    }
}
