namespace GymBooking_API.Entity
{
    public class OrderItem
    {
        public int Id { get; set; }


        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int PackageId { get; set; }
        public Package Package { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }



    }
}
