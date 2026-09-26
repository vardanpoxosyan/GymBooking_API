namespace GymBooking_API.DTO.Cart
{
    public sealed record  AddToCartDto
    {
        public int PackageId { get; set; }
        public int Quantity { get; set; }
    }
}
