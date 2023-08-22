using BusTransit.Shared;
using MassTransit;
using Bus = BusTransit.Shared.Bus;

namespace BusTransit.Publisher;

public class Publisher : BackgroundService
{
    private readonly ILogger<Publisher> _logger;
    readonly IBus _bus;
    
    public Publisher(ILogger<Publisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //await Task.Yield();
            
            Console.Write("Enter the bus number>> ");
            var busNumber = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(busNumber))
            {
                var bus = new Bus(Id: Guid.NewGuid(), Number: busNumber);
                _logger.LogInformation("The bus({}) has been dispatched", bus.ToString());
                await _bus.Publish(bus, stoppingToken);
            }
            
        }
    }
}