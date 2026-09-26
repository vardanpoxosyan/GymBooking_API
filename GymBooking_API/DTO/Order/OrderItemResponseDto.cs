namespace GymBooking_API.DTO.Order
{
    public sealed record OrderItemResponseDto
    {
        public int PackageId { get; set; }

        public string Packagname { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal Unitprice { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
