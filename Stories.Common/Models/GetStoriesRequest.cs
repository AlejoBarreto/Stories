namespace Stories.Common.Models;
public class GetStoriesRequest
{
    public string OrderBy { get; set; } = string.Empty;
    public int Limit { get; set; }
}
