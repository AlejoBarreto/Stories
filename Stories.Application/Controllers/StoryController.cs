using Microsoft.AspNetCore.Mvc;
using Stories.Common.Models;
using Stories.SharedKernel.Interfaces;

namespace Stories.Application.Controllers;

[ApiController]
[Route("stories-api/[controller]")]
public class StoryController : ControllerBase
{
    private readonly IStoryService _storyService;

    public StoryController(IStoryService storyService)
    {
        _storyService = storyService;
    }

    [HttpGet("GetStories")]
    [ProducesResponseType(typeof(List<GetStoryDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStories([FromQuery] GetStoriesRequest request)
    {
        var response = await _storyService.GetStoriesAsync(request);

        if (!response.Any())
            return BadRequest();

        return Ok(response);
    }

    [HttpGet("GetStory")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStoryById([FromQuery] int id)
    {
        var response = await _storyService.GetStoryDetailsAsync(id);

        if (response == null)
            return NotFound($"Story with Id: {id} not found.");

        return Ok(response);
    }

}
