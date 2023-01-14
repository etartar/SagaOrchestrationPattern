namespace Shared.Library.Messages
{
    public interface IStockRollBackMessage
    {
        int OrderId { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
