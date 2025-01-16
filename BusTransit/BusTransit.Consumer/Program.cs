using BusTransit.Consumer;
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
        
        cfg.ReceiveEndpoint("priority-orders", x =>
        {
            x.ConfigureConsumeTopology = false; 
            x.Bind("submitorder", s => 
            {
                s.RoutingKey = "PRIORITY";
                s.ExchangeType = ExchangeType.Direct;
            });
            x.ConfigureConsumer<FirstConsumer>(context);
        });

        cfg.ReceiveEndpoint("regular-orders", x =>
        {
            x.ConfigureConsumeTopology = false; 
            x.Bind("submitorder", s => 
            {
                s.RoutingKey = "REGULAR";
                s.ExchangeType = ExchangeType.Direct;
            });
            x.ConfigureConsumer<SecondConsumer>(context);
        });
    });
});


var app = builder.Build();

app.Run();