using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Library.Settings;
using Stock.API.Consumers;
using Stock.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StockDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("StockDbConnectionString"));
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

        cfg.ReceiveEndpoint(RabbitMQSettings.StockOrderCreatedEventQueueName, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(RabbitMQSettings.StockRollBackMessageQueueName, e =>
        {
            e.ConfigureConsumer<StockRollBackMessageConsumer>(context);
        });
    });

    mt.AddConsumer<OrderCreatedEventConsumer>();
    mt.AddConsumer<StockRollBackMessageConsumer>();
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
