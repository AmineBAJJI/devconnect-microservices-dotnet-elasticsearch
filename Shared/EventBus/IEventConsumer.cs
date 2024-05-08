namespace Shared.EventBus;

public interface IEventConsumer
{
    void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler);
    void StartSubscriptions();
    //void Unsubscribe<TEvent>() where TEvent : class;
}