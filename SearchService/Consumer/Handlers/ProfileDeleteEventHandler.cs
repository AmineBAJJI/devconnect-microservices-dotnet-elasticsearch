using System;
using System.Threading.Tasks;
using Nest;
using Shared.EventBus;
using Shared.Events.Profile;

namespace SearchService.Consumer.Handlers;

public class ProfileDeleteEventHandler : IEventHandler<ProfileDeleted>
{
    private readonly IElasticClient _client;
    private readonly string _indexName;

    public ProfileDeleteEventHandler(IElasticClient client, string indexName = "profiles")
    {
        _client = client;
        _indexName = indexName;
    }

    public async Task HandleAsync(ProfileDeleted @event)
    {
        Console.WriteLine($"Handling profile deletion event for profile ID: {@event.Id}");
        try
        {
            var deleteResponse =
                await _client.DeleteAsync<ProfileDeleted>(@event.Id, d => d.Index(_indexName));

            if (!deleteResponse.IsValid)
            {
                var errorMsg = "Problem deleting document from Elasticsearch.";
                Console.WriteLine(errorMsg);
                throw new Exception(errorMsg);
            }

            Console.WriteLine("Profile document deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling profile deletion event: {ex.Message}");
        }
    }
}