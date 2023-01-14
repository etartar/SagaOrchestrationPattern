using MassTransit;

namespace Shared.Library.Interfaces
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        string Reason { get; set; }
    }
}
