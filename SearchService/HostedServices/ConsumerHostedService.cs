using Shared.EventBus;

namespace SearchService.HostedServices;

/// Mlli tandiro inheritance direclty from BackgroundService we don't need to implement IHostedSerice,
/// w IDisposable, so running behavior kadiro hya
/// gher ila bghina n customiziwh
/// kadirhom hya, we need to focus just on logic of our long-running taks
/// by overriding ExecuteAsync
public class ConsumerHostedService : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private readonly ILogger<ConsumerHostedService> _logger;

    public ConsumerHostedService(IEventConsumer consumer, ILogger<ConsumerHostedService> logger)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConsumerHostedService started.");

        try
        {
            _consumer.StartSubscriptions();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("ConsumerHostedService was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in ConsumerHostedService.");
        }

        _logger.LogInformation("ConsumerHostedService stopped.");
    }
}