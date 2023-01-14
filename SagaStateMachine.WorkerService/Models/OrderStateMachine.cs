using Automatonymous;
using Shared.Library;
using Shared.Library.Events;
using Shared.Library.Interfaces;
using Shared.Library.Messages;
using Shared.Library.Settings;

namespace SagaStateMachine.WorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }
        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrenctState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, y => y.CorrelateById(x => x.CorrelationId, z => z.Message.CorrelationId));

            Event(() => StockNotReservedEvent, y => y.CorrelateById(x => x.CorrelationId, z => z.Message.CorrelationId));

            Event(() => PaymentCompletedEvent, y => y.CorrelateById(x => x.CorrelationId, z => z.Message.CorrelationId));

            Event(() => PaymentFailedEvent, y => y.CorrelateById(x => x.CorrelationId, z => z.Message.CorrelationId));

            Initially(When(OrderCreatedRequestEvent)
                .Then(context =>
                {
                    context.Instance.BuyerId = context.Data.BuyerId;
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.CreatedDate = DateTime.Now;

                    context.Instance.CardName = context.Data.Payment.CardName;
                    context.Instance.CardNumber = context.Data.Payment.CardNumber;
                    context.Instance.CVV = context.Data.Payment.CVV;
                    context.Instance.Expiration = context.Data.Payment.Expiration;
                    context.Instance.TotalPrice = context.Data.Payment.TotalPrice;
                })
                .Then(context =>
                {
                    Console.WriteLine($"OrderCreatedRequestEvent before : {context.Instance}");
                })
                .Publish(context =>
                {
                    return new OrderCreatedEvent(context.Instance.CorrelationId)
                    {
                        OrderItems = context.Data.OrderItems
                    };
                })
                .TransitionTo(OrderCreated)
                .Then(context =>
                {
                    Console.WriteLine($"OrderCreatedRequestEvent after : {context.Instance}");
                }));

            During(OrderCreated, When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{RabbitMQSettings.PaymentStockReservedRequestEventQueueName}"), context => new StockReservedRequestPaymentEvent(context.Instance.CorrelationId)
                {
                    OrderItems = context.Data.OrderItems,
                    Payment = new PaymentMessage
                    {
                        CardName = context.Instance.CardName,
                        CardNumber = context.Instance.CardNumber,
                        CVV = context.Instance.CVV,
                        Expiration = context.Instance.Expiration,
                        TotalPrice = context.Instance.TotalPrice
                    },
                    BuyerId = context.Instance.BuyerId
                })
                .Then(context =>
                {
                    Console.WriteLine($"StockReservedEvent after : {context.Instance}");
                }), When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Publish(context => new OrderRequestFailedEvent(context.Instance.OrderId, context.Data.Reason))
                .Then(context =>
                {
                    Console.WriteLine($"StockNotReservedEvent after : {context.Instance}");
                }));

            During(StockReserved, When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(context => new OrderRequestCompletedEvent(context.Instance.OrderId))
                .Then(context =>
                {
                    Console.WriteLine($"PaymentCompletedEvent after : {context.Instance}");
                }).Finalize(), When(PaymentFailedEvent)
                .Publish(context => new OrderRequestFailedEvent(context.Instance.OrderId, context.Data.Reason))
                .Send(new Uri($"queue:{RabbitMQSettings.StockRollBackMessageQueueName}"), context =>
                {
                    return new StockRollBackMessage(context.Instance.OrderId, context.Data.OrderItems);
                })
                .TransitionTo(PaymentFailed).Then(context =>
                {
                    Console.WriteLine($"PaymentFailedEvent after : {context.Instance}");
                }));

            SetCompletedWhenFinalized();
        }
    }
}
