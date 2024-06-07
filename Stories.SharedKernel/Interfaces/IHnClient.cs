using Stories.Common.Models;

namespace Stories.SharedKernel.Interfaces;
public interface IHnClient
{
    Task<List<int>?> GetNewestStoriesAsync(GetStoriesRequest request);

    Task<List<GetStoryDetailsResponse?>> GetNewestStoriesDetailsAsync(List<int> storiesIds);

    Task<GetStoryDetailsResponse?> GetStoryDetailsByIdAsync(int id);
}
