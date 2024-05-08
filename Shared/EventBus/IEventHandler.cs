namespace Shared.EventBus;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent @event);
}