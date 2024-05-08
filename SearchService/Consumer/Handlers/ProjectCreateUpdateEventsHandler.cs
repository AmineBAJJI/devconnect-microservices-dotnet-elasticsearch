using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Shared.EventBus;
using Shared.Events.Project;

namespace SearchService.Consumer.Handlers;

public class ProjectCreateUpdateEventsHandler : IEventHandler<ProjectDto>
{
    private readonly IElasticClient _client;
    private readonly string _indexName;

    public ProjectCreateUpdateEventsHandler(IElasticClient client, string indexName = "projects")
    {
        _client = client;
        _indexName = indexName;
    }

    public async Task HandleAsync(ProjectDto @event)
    {
        Console.WriteLine($"Handling project event for project ID: {@event.Id}");
        try
        {
            var documentExistsResponse =
                await _client.DocumentExistsAsync<ProjectDto>(@event.Id, d => d.Index(_indexName));

            if (documentExistsResponse.Exists)
            {
                var updateResponse = await _client.UpdateAsync<ProjectDto, object>(@event.Id, u => u
                    .Index(_indexName)
                    .Doc(@event)
                    .Refresh(Refresh.True));

                if (!updateResponse.IsValid)
                {
                    var errorMsg = "Problem updating document in Elasticsearch.";
                    Console.WriteLine(errorMsg);
                    throw new Exception(errorMsg);
                }

                Console.WriteLine("Project document updated successfully.");
            }
            else
            {
                var indexResponse = await _client.IndexAsync(@event, idx => idx.Index(_indexName));

                if (!indexResponse.IsValid)
                {
                    var errorMsg = "Problem inserting document to Elasticsearch.";
                    Console.WriteLine(errorMsg);
                    throw new Exception(errorMsg);
                }

                Console.WriteLine("Project document indexed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling project event: {ex.Message}");
        }
    }
}