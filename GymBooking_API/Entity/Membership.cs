namespace GymBooking_API.Entity
{
    /*Երբ User-y վճարում է գնելով Package ստեղծվում է Membership դարնալով Gym-Ի անդամ 
      հուշում է որ փաստ է որ User ունի այս Package
    Membership-ը ասում է ինչ իրավունք/աբոնեմենտ ունես։
     */

    //վճարաման համար թե User ընց է ուզում վճարել Օրական թե ամսեկան
    public enum MembershipType
    {
        Daily,
        Monthly
    }
    public class Membership
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;   }
        public MembershipType Type { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
