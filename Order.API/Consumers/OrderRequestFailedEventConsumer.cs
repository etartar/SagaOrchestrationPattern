using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Library.Interfaces;

namespace Order.API.Consumers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        private readonly ILogger<OrderRequestFailedEventConsumer> _logger;
        private readonly OrderDbContext _context;

        public OrderRequestFailedEventConsumer(ILogger<OrderRequestFailedEventConsumer> logger, OrderDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);

            if (order is not null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Reason;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id={context.Message.OrderId}) not found.");
            }
        }
    }
}
