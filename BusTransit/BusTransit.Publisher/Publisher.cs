using System.Security.Cryptography;
using BusTransit.Shared;
using MassTransit;

namespace BusTransit.Publisher;

public class Publisher : BackgroundService
{
    private readonly ILogger<Publisher> _logger;
    private readonly IBus _bus;

    public Publisher(ILogger<Publisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            // Console.Write("Enter the bus number>> ");
            // var busNumber = int.Parse(Console.ReadLine());
            var random = new Random();
            var busNumber = random.Next(101);
            if (busNumber<=50)
            {
                var firstBus = new MyMessage()
                {
                    Id = Guid.NewGuid(),
                    Number = busNumber,
                    
                };
                _logger.LogInformation("The bus({}) has been dispatched", firstBus.Number);
                await _bus.Publish(firstBus, context =>
                {
                    context.SetRoutingKey("key.first");
                }, cancellationToken: stoppingToken);
            }
            else
            {
                var secondBus = new MyMessage
                {
                    Id = Guid.NewGuid(),
                    Number = busNumber
                };
                _logger.LogInformation("The bus({}) has been dispatched",  secondBus.Number);
                await _bus.Publish(secondBus, context =>
                {
                    context.SetRoutingKey("key.second");
                }, cancellationToken: stoppingToken);
            }
        }
    }
}