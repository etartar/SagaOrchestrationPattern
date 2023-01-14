using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.WorkerService;
using SagaStateMachine.WorkerService.Models;
using Shared.Library.Settings;
using System.Reflection;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
                .EntityFrameworkRepository(opt =>
                {
                    opt.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                    {
                        builder.UseSqlServer(hostContext.Configuration.GetConnectionString("SagaDbConnectionString"), m =>
                        {
                            m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        });
                    });
                });

            cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                configure.Host(hostContext.Configuration.GetConnectionString("RabbitMQConnectionString"), "/", host =>
                {
                    host.Username(hostContext.Configuration.GetConnectionString("RabbitMQUserName"));
                    host.Password(hostContext.Configuration.GetConnectionString("RabbitMQPassword"));
                });

                configure.ReceiveEndpoint(RabbitMQSettings.OrderSaga, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });
            }));
        });

        services.AddMassTransitHostedService();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
