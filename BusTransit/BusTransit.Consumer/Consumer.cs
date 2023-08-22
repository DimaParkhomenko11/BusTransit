using BusTransit.Shared;
using MassTransit;
using Bus = BusTransit.Shared.Bus;

namespace BusTransit.Consumer;

public class Consumer : IConsumer<Bus>
{
    readonly ILogger<Consumer> _logger;

    public Consumer(ILogger<Consumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Bus> context)
    {
        _logger.LogInformation("The bus({}) has been arrived", context.Message );
        return Task.CompletedTask;
    }
}