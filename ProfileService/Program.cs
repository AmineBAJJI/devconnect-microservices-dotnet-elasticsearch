using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Repositories;
using RabbitMQ.Client;
using Shared.EventBus;

var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine(environmentName);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Profile")));

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//ISerivceProvider is responsible for resolving
//specify your own resolve function/factory method
// factory method
/*(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new RabbitMqEventPublisher(configuration);
});*/

builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();