using BusTransit.Shared;
using MassTransit;

namespace BusTransit.Consumer;

public class FirstConsumer : IConsumer<MyMessage>
{
    readonly ILogger<FirstConsumer> _logger;

    public FirstConsumer(ILogger<FirstConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MyMessage> context)
    {
        _logger.LogInformation("The bus({}) has been arrived in first consumer", context.Message.Number );
        return Task.CompletedTask;
    }
}