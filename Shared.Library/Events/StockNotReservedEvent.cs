using Shared.Library.Interfaces;

namespace Shared.Library.Events
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {
        public StockNotReservedEvent()
        {
        }

        public StockNotReservedEvent(Guid correlationId, string reason)
        {
            CorrelationId = correlationId;
            Reason = reason;
        }

        public Guid CorrelationId { get; set; }
        public string Reason { get; set; }
    }
}
