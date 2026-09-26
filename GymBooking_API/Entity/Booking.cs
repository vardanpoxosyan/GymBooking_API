namespace GymBooking_API.Entity
{
    //Ամրագրում ենք  տեղ ասյելով օրնակ YOGA դասի համար
    //Booking-ը ասում է՝ այդ իրավունքով որ կոնկրետ դասին ես գրանցվել։
    public class Booking
    {
        public int Id { get; set; }
        
        //Ո՞վ է ամրագրել
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        //Ի՞նչ դաս է ամրագրել
        public int GymClassId { get; set; }
        public Gym GymClass { get; set; }
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    }
}
