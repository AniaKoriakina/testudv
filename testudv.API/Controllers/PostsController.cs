using MediatR;
using Microsoft.AspNetCore.Mvc;
using testudv.Application.Commands;

namespace testudv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] string domain, [FromQuery] int count)
    {
        var query = new GetPostsCommand { Domain = domain, Count = count };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddPostLetters([FromQuery] string domain, [FromQuery] int count)
    {
        var query = new CreatePostInfoCommand { Domain = domain, Count = count };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}