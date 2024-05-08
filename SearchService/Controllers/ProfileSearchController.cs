using Microsoft.AspNetCore.Mvc;
using Nest;
using Shared.Events.Profile;

namespace SearchService.Controllers;

[ApiController]
[Route("profiles")]
public class ProfileSearchController : ControllerBase
{
    private readonly IElasticClient _client;
    private readonly string _indexName;

    public ProfileSearchController(IElasticClient client, string indexName = "profiles")
    {
        _client = client;
        _indexName = indexName;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProfiles([FromQuery] string query)
    {
        try
        {
            var response = await _client.SearchAsync<ProfileDocument>(s => s
                .Index(_indexName)
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(query)
                    )
                )
            );

            if (response.IsValid)
            {
                var profiles = new List<ProfileDocument>();
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