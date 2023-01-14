using Shared.Library.Interfaces;

namespace Shared.Library.Events
{
    public class OrderRequestCompletedEvent : IOrderRequestCompletedEvent
    {
        public OrderRequestCompletedEvent(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; set; }
    }
}
