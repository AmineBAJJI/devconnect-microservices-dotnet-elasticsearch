using Nest;

namespace SearchService.Extensions;

public static class ElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:uri"]));

        var defaultIndex = configuration["ElasticsearchSettings:defaultIndex"];

        if (!string.IsNullOrEmpty(defaultIndex)) 
            settings = settings.DefaultIndex(defaultIndex);

        var apiKeyId = configuration["ElasticsearchSettings:apiKeyId"];
        var apiKey = configuration["ElasticsearchSettings:apiKey"];

        if (!string.IsNullOrEmpty(apiKeyId) && !string.IsNullOrEmpty(apiKey))
        {
            settings = settings.ApiKeyAuthentication(apiKeyId, apiKey);
        }
        else
        {
            var basicAuthUser = configuration["ElasticsearchSettings:basicAuthUser"];
            var basicAuthPassword = configuration["ElasticsearchSettings:basicAuthPassword"];

            if (!string.IsNullOrEmpty(basicAuthUser) && !string.IsNullOrEmpty(basicAuthPassword))
                settings = settings.BasicAuthentication(basicAuthUser, basicAuthPassword);
        }

        var client = new ElasticClient(settings);
        
        services.AddSingleton<IElasticClient>(client);
    }
}