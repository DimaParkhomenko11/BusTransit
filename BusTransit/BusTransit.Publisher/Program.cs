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
        
        cfg.Publish<MyMessage>(publishConfig =>
        {
            publishConfig.ExchangeType = "topic";
        });
    });
});

builder.Services.AddHostedService<Publisher>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();