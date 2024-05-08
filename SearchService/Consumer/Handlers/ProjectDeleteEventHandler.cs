using System;
using System.Threading.Tasks;
using Nest;
using Shared.EventBus;
using Shared.Events.Project;

namespace SearchService.Consumer.Handlers;

public class ProjectDeleteEventHandler : IEventHandler<ProjectDeleteDto>
{
    private readonly IElasticClient _client;
    private readonly string _indexName;

    public ProjectDeleteEventHandler(IElasticClient client, string indexName = "projects")
    {
        _client = client;
        _indexName = indexName;
    }

    public async Task HandleAsync(ProjectDeleteDto @event)
    {
        Console.WriteLine($"Handling project deletion event for project ID: {@event.Id}");
        try
        {
            var deleteResponse =
                await _client.DeleteAsync<ProjectDeleteDto>(@event.Id, d => d.Index(_indexName));

            if (!deleteResponse.IsValid)
            {
                var errorMsg = "Problem deleting document from Elasticsearch.";
                Console.WriteLine(errorMsg);
                throw new Exception(errorMsg);
            }

            Console.WriteLine("Project document deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling project deletion event: {ex.Message}");
        }
    }
}