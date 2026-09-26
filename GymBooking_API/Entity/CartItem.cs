namespace GymBooking_API.Entity
{
    //Այց դասը Զաբյուղի մեջ գնտվող ապրանքներ են
    public class CartItem
    {
        public int Id { get; set; }

        //Որ զամբյուղի մեջ կարող է լինել
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        // Սա հանդիսանում է այն ապրանքը կամ ծառայությունը որը վաճառումե ենք մեր նախագծում մեր վաճառում ենք Package
        public int PackagId { get; set; }
        public Package Package { get; set; } = null!;
        //Ապրանքի քանակ
        public int Quantity { get; set; }
    }
}
