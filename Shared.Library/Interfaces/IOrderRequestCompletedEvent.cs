using MassTransit;

namespace Shared.Library.Interfaces
{
    public interface IOrderRequestCompletedEvent
    {
        int OrderId { get; set; }
    }
}
