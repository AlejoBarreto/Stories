using Stories.Common.Models;

namespace Stories.SharedKernel.Interfaces;
public interface IStoryRepository
{
    Task<List<GetStoryDetailsResponse?>> GetStoriesAsync(GetStoriesRequest request);

    Task<GetStoryDetailsResponse?> GetStoryDetailsAsync(int id);
}