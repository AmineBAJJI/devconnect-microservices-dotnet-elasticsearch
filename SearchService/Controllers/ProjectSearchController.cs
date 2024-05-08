using Microsoft.AspNetCore.Mvc;
using Nest;
using Shared.Events.Project;

namespace SearchService.Controllers;

[ApiController]
[Route("projects")]
public class ProjectSearchController : ControllerBase
{
    private readonly IElasticClient _client;
    private readonly string _indexName;
    public ProjectSearchController(IElasticClient client, string indexName = "projects")
    {
        _client = client;
        _indexName = indexName;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProfiles([FromQuery] string query)
    {
        try
        {
            var response = await _client.SearchAsync<ProjectDto>(s => s
                .Index(_indexName)
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(query)
                    )
                )
            );

            if (response.IsValid)
            {
                var profiles = new List<ProjectDto>();
                foreach (var hit in response.Hits)
                {
                    profiles.Add(hit.Source);
                }

                return Ok(profiles);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error searching profiles: {ex.Message}");
        }
    }
}