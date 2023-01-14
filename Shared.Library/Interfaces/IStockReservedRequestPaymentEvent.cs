using MassTransit;

namespace Shared.Library.Interfaces
{
    public interface IStockReservedRequestPaymentEvent : CorrelatedBy<Guid>
    {
        PaymentMessage Payment { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
        string BuyerId { get; set; }
    }
}
