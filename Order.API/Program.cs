using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared.Library.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OrderDbConnectionString"));
});

builder.Services.AddMassTransit(mt =>
{
    mt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQConnectionString"), "/", host =>
        {
            host.Username(builder.Configuration.GetConnectionString("RabbitMQUserName"));
            host.Password(builder.Configuration.GetConnectionString("RabbitMQPassword"));
        });

        cfg.ReceiveEndpoint(RabbitMQSettings.OrderRequestCompletedEventQueueName, e =>
        {
            e.ConfigureConsumer<OrderRequestCompletedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(RabbitMQSettings.OrderRequestFailedEventQueueName, e =>
        {
            e.ConfigureConsumer<OrderRequestFailedEventConsumer>(context);
        });
    });

    mt.AddConsumer<OrderRequestCompletedEventConsumer>();
    mt.AddConsumer<OrderRequestFailedEventConsumer>();
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
