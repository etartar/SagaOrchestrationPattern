using MassTransit;

namespace Shared.Library.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}
