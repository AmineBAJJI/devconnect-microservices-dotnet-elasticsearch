using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Shared.EventBus;

public class Test
{
    public int _testfield { get; set; }

    public Test(int testfield)
    {
        _testfield = testfield;
    }
}

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqEventPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        var rabbitMqConfig = _configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>();
        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMqConfig.HostName,
            UserName = rabbitMqConfig.UserName,
            Password = rabbitMqConfig.Password,
            VirtualHost = rabbitMqConfig.VirtualHost
        };
        using var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        var eventName = typeof(TEvent).Name;
        var queueName = RabbitMqNamingStrategy.GetQueueName(eventName);
        var exchangeName = RabbitMqNamingStrategy.GetExchangeName(eventName);

        channel.QueueDeclare(queueName, exclusive: false, durable: true, autoDelete: false);
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false);
        channel.QueueBind(queueName, exchangeName, exchangeName + "_To_" + queueName);
        var messageBody = SerializeEvent(@event);

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: exchangeName + "_To_" + queueName,
            basicProperties: null,
            body: messageBody);
    }

    private byte[] SerializeEvent<TEvent>(TEvent @event)
    {
        try
        {
            var message = JsonConvert.SerializeObject(@event);
            return Encoding.UTF8.GetBytes(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error serializing event: {ex.Message}");
            throw;
        }
    }
}