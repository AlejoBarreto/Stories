using System.Net.Http.Json;
using Stories.Common.Models;
using Stories.SharedKernel.Interfaces;

namespace Stories.Infrastructure.Client;
public class HnClient : IHnClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    public const string ClientName = "StoriesApiClient";

    public HnClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<int>?> GetNewestStoriesAsync(GetStoriesRequest request)
    {
        var client = _httpClientFactory.CreateClient(ClientName);

        var response = await client.GetAsync(
            $"newstories.json?print=pretty&orderBy=\"${request.OrderBy}\"&limitToFirst={request.Limit}");

        var storiesIds = await response.Content.ReadFromJsonAsync<List<int>>();

        return storiesIds;
    }

    public async Task<List<GetStoryDetailsResponse?>> GetNewestStoriesDetailsAsync(List<int> storiesIds)
    {
        List<GetStoryDetailsResponse?> stories = new();

        var client = _httpClientFactory.CreateClient(ClientName);

        var tasks = storiesIds.Select(async storyId =>
        {
            var detailResponse = await client.GetAsync($"item/{storyId}.json?print=pretty");
            var detail = await detailResponse.Content.ReadFromJsonAsync<GetStoryDetailsResponse>();
            return detail;
        });

        var details = await Task.WhenAll(tasks);

        stories.AddRange(details.Where(detail => detail != null));

        return stories;
    }

    public async Task<GetStoryDetailsResponse?> GetStoryDetailsByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient(ClientName);

        var response = await client.GetAsync($"item/{id}.json?print=pretty");

        GetStoryDetailsResponse? storyDetails = await response.Content.ReadFromJsonAsync<GetStoryDetailsResponse>();

        return storyDetails;
    }
}