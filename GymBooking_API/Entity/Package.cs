namespace GymBooking_API.Entity
{
    public enum Includes
    {
        Gym=1,
        GymPlusPool,GymPlusCoach,GymPlusCoachPlusPool,
    }
    // Ես են պաթեթներն են որ առաջրակում ենք հաճախորդին
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Includes Includes { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        // Սա նշանակում է որ 1 փաթեթը կարող է վաճառվել շատ անգամներ
        //Ովքեր են գնել այս Package
        public ICollection<Membership> Memberships { get; set; }= new List<Membership>();
        //Փաթեում ինչ դասեր կան Many-to many relationship
        public ICollection<Gym> GymClasses { get; set; }= new List<Gym>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
