using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using testudv.Application.Commands;
using testudv.Application.Exceptions;

namespace testudv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<GetPostsCommand> _validatorPosts;
    private readonly IValidator<CreatePostsInfoCommand> _validatorPostsInfo;

    public PostsController(IMediator mediator, IValidator<GetPostsCommand> validatorPosts, IValidator<CreatePostsInfoCommand> validatorPostsInfo)
    {
        _mediator = mediator;
        _validatorPosts = validatorPosts;
        _validatorPostsInfo = validatorPostsInfo;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] string domain, [FromQuery] int count)
    {
        var query = new GetPostsCommand { Domain = domain, Count = count };
        var validationResult = _validatorPosts.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.First().ErrorMessage);
        }
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (VkApiException ex)
        {
            if (ex.ErrorCode == 100)
            {
                return BadRequest($"Неверный домен: {ex.ErrorMessage}");
            }
            return StatusCode(500, $"Ошибка VK API: {ex.ErrorMessage} (Code: {ex.ErrorCode})");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Неизвестная ошибка: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPostLetters([FromQuery] string domain, [FromQuery] int count)
    {
        var query = new CreatePostsInfoCommand { Domain = domain, Count = count };
        var validationResult = _validatorPostsInfo.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (VkApiException ex)
        {
            if (ex.ErrorCode == 100)
            {
                return BadRequest($"Неверный домен: {ex.ErrorMessage}");
            }
            return StatusCode(500, $"Ошибка VK API: {ex.ErrorMessage} (Code: {ex.ErrorCode})");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Неизвестная ошибка: {ex.Message}");
        }
    }
}