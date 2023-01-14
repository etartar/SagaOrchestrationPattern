namespace Shared.Library
{
    public class OrderItemMessage
    {
        public OrderItemMessage()
        {
        }

        public OrderItemMessage(int productId, int count)
        {
            ProductId = productId;
            Count = count;
        }

        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
