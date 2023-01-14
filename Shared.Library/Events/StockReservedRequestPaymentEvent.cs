using Shared.Library.Interfaces;

namespace Shared.Library.Events
{
    public class StockReservedRequestPaymentEvent : IStockReservedRequestPaymentEvent
    {
        public StockReservedRequestPaymentEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string BuyerId { get; set; }
    }
}
