using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Shared.EventBus;
using Shared.Events.Profile;

namespace SearchService.Consumer.Handlers
{
    public class ProfileCreateUpdateEventsHandler : IEventHandler<ProfileDocument>
    {
        private readonly IElasticClient _client;
        private readonly string _indexName;

        public ProfileCreateUpdateEventsHandler(IElasticClient client, string indexName = "profiles")
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _indexName = indexName;
        }

        public async Task HandleAsync(ProfileDocument @event)
        {
            Console.WriteLine($"Handling profile event for profile ID: {@event.Id}");
            try
            {
                var documentExistsResponse =
                    await _client.DocumentExistsAsync<ProfileDocument>(@event.Id, d => d.Index(_indexName));
                Console.WriteLine(@event.FirstName);
                if (documentExistsResponse.Exists)
                {
                    var updateResponse = await _client.UpdateAsync<ProfileDocument, object>(@event.Id, u => u
                        .Index(_indexName)
                        .Doc(@event)
                        .Refresh(Refresh.True));

                    if (!updateResponse.IsValid)
                    {
                        var errorMsg = "Problem updating document in Elasticsearch.";
                        Console.WriteLine(errorMsg);
                        throw new Exception(errorMsg);
                    }

                    Console.WriteLine("Profile document updated successfully.");
                }
                else
                {
                   
                    var indexResponse = await _client.IndexAsync(@event, idx => idx.Index(_indexName));
                    Console.WriteLine("Je suis ici");
                    if (!indexResponse.IsValid)
                    {
                        var errorMsg = "Problem inserting document to Elasticsearch.";
                        Console.WriteLine(errorMsg);
                        throw new Exception(errorMsg);
                    }
                    Console.WriteLine("Profile document indexed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling profile event: {ex.Message}");
            }
        }
    }
}
