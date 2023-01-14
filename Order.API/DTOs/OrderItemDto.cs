namespace Order.API.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
