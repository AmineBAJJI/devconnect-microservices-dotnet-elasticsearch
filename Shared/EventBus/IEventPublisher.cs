namespace Shared.EventBus;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event);
}