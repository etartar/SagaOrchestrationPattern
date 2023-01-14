using MassTransit;
using Shared.Library.Events;
using Shared.Library.Interfaces;

namespace Payment.API.Consumers
{
    public class StockReservedRequestPaymentEventConsumer : IConsumer<IStockReservedRequestPaymentEvent>
    {
        private readonly ILogger<StockReservedRequestPaymentEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedRequestPaymentEventConsumer(ILogger<StockReservedRequestPaymentEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestPaymentEvent> context)
        {
            /// ÖRnek müşteri bakiyesi belirliyoruz.
            decimal balance = 3000m;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for user id= {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was not widthdrawn from credit card for user id= {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enough balance",
                    OrderItems = context.Message.OrderItems
                });
            }
        }
    }
}
