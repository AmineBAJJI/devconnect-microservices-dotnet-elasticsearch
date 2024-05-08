using System.Text;
using Nest;
using Newtonsoft.Json;
using Shared.Events.Project;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SearchService.Consumer.Handlers;
using Shared.EventBus;
using Shared.Events.Profile;

namespace SearchService.Consumer;

public class RabbitMqEventConsumer : IEventConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IElasticClient _elasticClient;

    public RabbitMqEventConsumer(IConfiguration configuration, IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
        var rabbitMqConfig = configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>();
        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMqConfig.HostName,
            UserName = rabbitMqConfig.UserName,
            Password = rabbitMqConfig.Password,
            VirtualHost = rabbitMqConfig.VirtualHost
        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
    {
        var eventName = typeof(TEvent).Name;
        var queueName = RabbitMqNamingStrategy.GetQueueName(eventName);
        var exchangeName = RabbitMqNamingStrategy.GetExchangeName(eventName);
        _channel.QueueDeclare(queueName, exclusive: false, durable: true, autoDelete: false);
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false);
        _channel.QueueBind(queueName, exchangeName, exchangeName + "_To_" + queueName);


        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var deserializedEvent = JsonConvert.DeserializeObject<TEvent>(body);
                await eventHandler.HandleAsync(deserializedEvent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    public void StartSubscriptions()
    {
        //profiles
        Subscribe<ProfileDocument>(new ProfileCreateUpdateEventsHandler(_elasticClient));
        Subscribe<ProfileDeleted>(new ProfileDeleteEventHandler(_elasticClient));

        //projects
        Subscribe<ProjectDto>(new ProjectCreateUpdateEventsHandler(_elasticClient));
        Subscribe<ProjectDeleteDto>(new ProjectDeleteEventHandler(_elasticClient));
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}