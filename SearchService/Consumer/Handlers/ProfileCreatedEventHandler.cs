/*using Nest;
using Shared.EventBus;
using Shared.Events.Profile;

namespace SearchService.Consumer.Handlers;

public class ProfileCreatedEventHandler : IEventHandler<ProfileCreated>
{
    private readonly IElasticClient _client;
    private readonly string _indexName;
    private readonly ILogger<ProfileCreatedEventHandler> _logger;
    public ProfileCreatedEventHandler(IElasticClient client, string indexName = "profiles")
    {
        _client = client;
        _indexName = indexName;
    }

    public async Task HandleAsync(ProfileDocument @event)
    {
        Console.WriteLine($"Profile created: {@event.Username}");
        try
        {
            var insertResponse = await _client.IndexAsync(@event, i => i
                    .Index("profiles")
                );
            
            if (!insertResponse.IsValid)
            {
                var errorMsg = "Problem inserting document to Elasticsearch.";
                //_logger.LogError(insertResponse.OriginalException, errorMsg);
                throw new Exception(errorMsg);
            }
            Console.WriteLine("Profile document indexed successfully.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error indexing profile: {ex.Message}");
        }
    }
}*/