namespace GymBooking_API.DTO.Cart
{
    public sealed record CartResponseDto
    {
        public int CartId { get; set; }
        public ICollection<CartItemResponseDto> Items { get; set; } = new List<CartItemResponseDto>();
    }
}
