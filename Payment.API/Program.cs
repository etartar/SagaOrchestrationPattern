using MassTransit;
using Payment.API.Consumers;
using Shared.Library.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(mt =>
{
    mt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQConnectionString"), "/", host =>
        {
            host.Username(builder.Configuration.GetConnectionString("RabbitMQUserName"));
            host.Password(builder.Configuration.GetConnectionString("RabbitMQPassword"));
        });

        cfg.ReceiveEndpoint(RabbitMQSettings.PaymentStockReservedRequestEventQueueName, e =>
        {
            e.ConfigureConsumer<StockReservedRequestPaymentEventConsumer>(context);
        });
    });

    mt.AddConsumer<StockReservedRequestPaymentEventConsumer>();
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
