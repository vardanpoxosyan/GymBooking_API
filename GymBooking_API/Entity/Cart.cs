namespace GymBooking_API.Entity
{
    //Սա հանդիսնում է զաբյուղը որով գնումենր է կատարում մեր User-ը
    //1 User Ի համար ունենք 1 քարտ  
    public class Cart
    {
        public int Id {  get; set; }
        
        //Սա տեղեկացնում է թե որ User-ին է պատկանում այս զաբյուղը
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public ICollection<CartItem> CartItems { get;set;  }=new List<CartItem>();  

    }
}
