using BusTransit.Publisher;
using MassTransit;
using Bus = BusTransit.Shared.Bus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddHostedService<Publisher>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();