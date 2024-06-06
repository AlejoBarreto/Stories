using System.Net.Http.Json;
using Stories.Common.Models;
using Stories.SharedKernel.Interfaces;

namespace Stories.Infrastructure.Repository;

public class StoryRepository : IStoryRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    public const string ClientName = "StoriesApiClient";
    public StoryRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<GetStoryDetailsResponse?>> GetStoriesAsync(GetStoriesRequest request)
    {
        List<GetStoryDetailsResponse?> stories = new();

        try
        {
            var client = _httpClientFactory.CreateClient(ClientName);

            var response = await client.GetAsync(
                $"newstories.json?print=pretty&orderBy=\"${request.OrderBy}\"&limitToFirst={request.Limit}");

            var storiesIds = await response.Content.ReadFromJsonAsync<List<int>>();

            if (storiesIds != null)
            {
                var tasks = storiesIds.Select(async storyId =>
                {
                    var detailResponse = await client.GetAsync($"item/{storyId}.json?print=pretty");
                    var detail = await detailResponse.Content.ReadFromJsonAsync<GetStoryDetailsResponse>();
                    return detail;
                });

                var details = await Task.WhenAll(tasks);

                stories.AddRange(details.Where(detail => detail != null));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error trying to get best stories: " + ex.Message);
        }

        return stories;
    }

    public async Task<GetStoryDetailsResponse?> GetStoryDetailsAsync(int id)
    {
        GetStoryDetailsResponse? storyDetails = null;
        try
        {
            var client = _httpClientFactory.CreateClient(ClientName);

            var response = await client.GetAsync($"item/{id}.json?print=pretty");

            storyDetails = await response.Content.ReadFromJsonAsync<GetStoryDetailsResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error trying to story details.");
        }

        return storyDetails;
    }
}
