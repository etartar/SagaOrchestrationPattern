using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Library.Events;
using Shared.Library.Interfaces;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly StockDbContext _dbContext;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(StockDbContext dbContext, ILogger<OrderCreatedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();

            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await _dbContext.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }

            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock != null)
                    {
                        stock.Count -= item.Count;

                        await _dbContext.SaveChangesAsync();
                    }
                }
                
                _logger.LogInformation($"Stock was reserved for CorrelationId Id: {context.Message.CorrelationId}");

                await _publishEndpoint.Publish(new StockReservedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderItems = context.Message.OrderItems
                });
            }
            else
            {
                _logger.LogInformation($"Not enough stock for CorrelationId Id: {context.Message.CorrelationId}");

                await _publishEndpoint.Publish(new StockNotReservedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    Reason = "Not enough stock"
                });
            }
        }
    }
}
