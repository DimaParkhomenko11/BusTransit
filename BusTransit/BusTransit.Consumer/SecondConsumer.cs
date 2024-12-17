using BusTransit.Shared;
using MassTransit;

public class SecondConsumer : IConsumer<MyMessage>
{
    readonly ILogger<SecondConsumer> _logger;

    public SecondConsumer(ILogger<SecondConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MyMessage> context)
    {
        _logger.LogInformation("The bus({}) has been arrived in second consumer", context.Message.Number );
        return Task.CompletedTask;
    }
}