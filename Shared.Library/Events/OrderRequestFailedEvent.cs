using Shared.Library.Interfaces;

namespace Shared.Library.Events
{
    public class OrderRequestFailedEvent : IOrderRequestFailedEvent
    {
        public OrderRequestFailedEvent(int orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
        }

        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}
