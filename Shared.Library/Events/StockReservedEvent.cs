using Shared.Library.Interfaces;

namespace Shared.Library.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public StockReservedEvent()
        {
        }

        public StockReservedEvent(Guid correlationId, List<OrderItemMessage> orderItems)
        {
            CorrelationId = correlationId;
            OrderItems = orderItems;
        }

        public Guid CorrelationId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
