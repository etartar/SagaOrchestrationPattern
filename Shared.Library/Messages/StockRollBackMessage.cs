namespace Shared.Library.Messages
{
    public class StockRollBackMessage : IStockRollBackMessage
    {
        public StockRollBackMessage(int orderId, List<OrderItemMessage> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

        public int OrderId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
