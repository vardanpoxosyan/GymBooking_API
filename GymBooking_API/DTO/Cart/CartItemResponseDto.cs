namespace GymBooking_API.DTO.Cart
{
    public sealed record CartItemResponseDto
    {
        public int PackageId { get; set; }
        public string PackagName { get; set; } = string.Empty;
        public decimal DailyPrice { get; set; }
        public decimal MonthlyPrice { get; set; }
        public int Quntity { get; set; }

    }
}
