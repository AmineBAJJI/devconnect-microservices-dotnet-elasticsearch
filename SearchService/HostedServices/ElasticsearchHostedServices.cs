using Nest;
using Shared.Events.Project;
using Shared.Events.Profile;

public class ElasticsearchHostedServices : BackgroundService
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<ElasticsearchHostedServices> _logger;

    public ElasticsearchHostedServices(IElasticClient elasticClient, ILogger<ElasticsearchHostedServices> logger)
    {
        _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var profilesIndexName = "profiles";
        var projectsIndexName = "projects";
        _logger.LogInformation("ElasticsearchHostedServices running.");

        try
        {
            // Verify profiles index
            if (!(await _elasticClient.Indices.ExistsAsync(profilesIndexName, ct: stoppingToken)).Exists)
            {
                _logger.LogInformation("Creating index: {IndexName}", profilesIndexName);
                var createIndexResponse = await _elasticClient.Indices.CreateAsync(profilesIndexName, c => c
                    .Map<ProfileDocument>(m => m
                        .AutoMap()
                    ), stoppingToken);

                if (!createIndexResponse.IsValid)
                {
                    _logger.LogError("Failed to create index: {IndexName}. Reason: {Reason}", profilesIndexName,
                        createIndexResponse.ServerError.Error);
                }
                else
                {
                    _logger.LogInformation("Index {IndexName} created successfully.", profilesIndexName);
                }
            }
            else
            {
                _logger.LogInformation("Index {IndexName} already exists.", profilesIndexName);
            }

            // Verify projects index
            if (!(await _elasticClient.Indices.ExistsAsync(projectsIndexName, ct: stoppingToken)).Exists)
            {
                _logger.LogInformation("Creating index: {IndexName}", projectsIndexName);
                var createIndexResponse = await _elasticClient.Indices.CreateAsync(projectsIndexName, c => c
                    .Map<ProjectDto>(m => m
                        .AutoMap()
                    ), stoppingToken);

                if (!createIndexResponse.IsValid)
                {
                    _logger.LogError("Failed to create index: {IndexName}. Reason: {Reason}", projectsIndexName,
                        createIndexResponse.ServerError.Error);
                }
                else
                {
                    _logger.LogInformation("Index {IndexName} created successfully.", projectsIndexName);
                }
            }
            else
            {
                _logger.LogInformation("Index {IndexName} already exists.", projectsIndexName);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Index creation operation was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating indexes.");
        }
    }
}