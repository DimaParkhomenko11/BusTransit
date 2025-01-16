using BusTransit.Publisher;
using BusTransit.Shared;
using MassTransit;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.Send<MyMessage>(x =>
        {
            // use customerType for the routing key
            x.UseRoutingKeyFormatter(context => context.Message.CustomerType);
        });
        
        cfg.Message<MyMessage>(x => x.SetEntityName("submitorder"));


        cfg.Publish<MyMessage>(x => x.ExchangeType = ExchangeType.Direct);
    });
});

builder.Services.AddHostedService<Publisher>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();