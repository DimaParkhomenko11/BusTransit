using BusTransit.Consumer;
using BusTransit.Shared;
using MassTransit;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<FirstConsumer>();
    x.AddConsumer<SecondConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        
        // Підключення FirstConsumer до першого routing key
        cfg.ReceiveEndpoint("queue.first", e =>
        {
            e.ConfigureConsumeTopology = false; // Вимикаємо автоматичну топологію
            e.Bind<MyMessage>(s =>
            {
                s.RoutingKey = "key.first"; 
                s.ExchangeType = ExchangeType.Topic;
            });
            e.ConfigureConsumer<FirstConsumer>(context);
        });

        // Підключення SecondConsumer до іншого routing key
        cfg.ReceiveEndpoint("queue.second", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind<MyMessage>( s =>
            {
                s.RoutingKey = "key.second"; // Прив'язуємо інший routing key
                s.ExchangeType = ExchangeType.Topic;
            });
            e.ConfigureConsumer<SecondConsumer>(context);
        });
    });
});


var app = builder.Build();

app.Run();