using Shared.Library.Interfaces;

namespace Shared.Library.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public PaymentFailedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
        public string Reason { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
