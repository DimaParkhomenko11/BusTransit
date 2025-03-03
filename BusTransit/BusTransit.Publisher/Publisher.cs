using System.Security.Cryptography;
using BusTransit.Shared;
using MassTransit;

namespace BusTransit.Publisher;

public class Publisher : BackgroundService
{
    private readonly ILogger<Publisher> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public Publisher(ILogger<Publisher> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            Console.Write("Enter the bus number>> ");
            var busNumber = int.Parse(Console.ReadLine());
            // var random = new Random();
            // var busNumber = random.Next(101);
            if (busNumber<=50)
            {
                var firstBus = new MyMessage()
                {
                    Id = Guid.NewGuid(),
                    Number = busNumber,
                    CustomerType = "PRIORITY"
                };
                _logger.LogInformation("The bus({}) has been dispatched", firstBus.Number);
                await _publishEndpoint.Publish(firstBus, cancellationToken: stoppingToken);
            }
            else
            {
                var secondBus = new MyMessage
                {
                    Id = Guid.NewGuid(),
                    Number = busNumber,
                    CustomerType = "REGULAR"
                };
                _logger.LogInformation("The bus({}) has been dispatched",  secondBus.Number);
                await _publishEndpoint.Publish(secondBus, cancellationToken: stoppingToken);
            }
        }
    }
}