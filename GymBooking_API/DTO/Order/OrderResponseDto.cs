namespace GymBooking_API.DTO.Order
{
    public sealed record OrderResponseDto
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemResponseDto> OrderItems = new List<OrderItemResponseDto>();

    }
}
