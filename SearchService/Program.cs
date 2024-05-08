using SearchService.Consumer;
using SearchService.Extensions;
using Shared.EventBus;
using SearchService.HostedServices;
    
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEventConsumer, RabbitMqEventConsumer>();


builder.Services.AddElasticsearch(builder.Configuration);
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddHostedService<ElasticsearchHostedServices>();
// Add controllers services
builder.Services.AddControllers();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();


