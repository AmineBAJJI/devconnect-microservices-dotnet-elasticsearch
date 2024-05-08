namespace Shared.EventBus;

public static class RabbitMqNamingStrategy
{
    public static string GetQueueName(string eventName)
    {
        return $"{eventName}_Queue";
    }

    public static string GetExchangeName(string eventName)
    {
        return $"{eventName}_Exchange";
    }
   
}