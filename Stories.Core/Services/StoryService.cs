using Microsoft.Extensions.Caching.Memory;
using Stories.Common.Models;
using Stories.SharedKernel.Interfaces;

namespace Stories.Core.Services;
public class StoryService : IStoryService
{
    private readonly IStoryRepository _storyRepository;
    private readonly IMemoryCache _cache;

    public StoryService(IStoryRepository storyRepository, IMemoryCache cache)
    {
        _cache = cache;
        _storyRepository = storyRepository;
    }

    public async Task<List<GetStoryDetailsResponse?>> GetStoriesAsync(GetStoriesRequest request)
    {

        var categories = await _cache.GetOrCreateAsync("categoriesList", async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(30);
            var categories = await _storyRepository.GetStoriesAsync(request);
            return categories;
        });

        return categories!;
    }

    public async Task<GetStoryDetailsResponse?> GetStoryDetailsAsync(int id)
    {
        return await _storyRepository.GetStoryDetailsAsync(id);
    }

}
