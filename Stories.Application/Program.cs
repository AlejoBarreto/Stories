using Stories.Common.Models;
using Stories.Core.Services;
using Stories.Infrastructure.Client;
using Stories.Infrastructure.Repository;
using Stories.SharedKernel.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IStoryService, StoryService>();
builder.Services.AddTransient<IStoryRepository, StoryRepository>();

builder.Services.AddSingleton<IHnClient, HnClient>();

builder.Services.AddMemoryCache();

var configSettings = builder.Configuration.GetSection(StoriesSettings.OptionKey).Get<StoriesSettings>();

builder.Services.AddHttpClient(HnClient.ClientName, httpClient =>
{
    httpClient.BaseAddress = new Uri(configSettings!.BaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(applicationBuilder =>
{
    applicationBuilder.AllowAnyMethod();
    applicationBuilder.AllowAnyHeader();
    applicationBuilder.AllowCredentials();
    applicationBuilder.SetIsOriginAllowed(_ => true);
});

app.UseAuthorization();

app.MapControllers();

app.Run();
