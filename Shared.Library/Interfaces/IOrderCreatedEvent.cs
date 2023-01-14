using MassTransit;

namespace Shared.Library.Interfaces
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
