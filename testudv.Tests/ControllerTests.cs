using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using testudv.API.Controllers;
using testudv.Application.Commands;
using testudv.Application.Dtos;
using testudv.Application.Validators;
using Xunit;

namespace testudv.Tests;

public class PostsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PostsController _controller;
    private readonly IValidator<GetPostsCommand> _validatorPost;
    private readonly IValidator<CreatePostsInfoCommand> _validatorPostInfo;

    public PostsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _validatorPost = new GetPostsValidator();
        _validatorPostInfo = new CreatePostsInfoValidator();
        _controller = new PostsController(_mediatorMock.Object, _validatorPost, _validatorPostInfo);
    }

    [Fact]
    public async Task GetPosts_ValidDomainAndCount_ReturnsOkResult()
    {
        var domain = "test_domain";
        var count = 5;
        var expectedResult = "longtext";
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPostsCommand>(), default))
            .ReturnsAsync(expectedResult);
        var result = await _controller.GetPosts(domain, count);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsAssignableFrom<string>(okResult.Value);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task GetPosts_InvalidDomainFormat_ReturnsBadRequest()
    {
        var domain = "invalid_domain@@@";
        var count = 5;
        var result = await _controller.GetPosts(domain, count);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var actualMessage = badRequestResult.Value.ToString();
        var expectedMessage = "Домен должен быть действительным.";

        Assert.Equal(expectedMessage, actualMessage);
    }

    [Fact]
    public async Task GetPosts_CountLessThanOrEqualToZero_ReturnsBadRequest()
    {
        var domain = "test_domain";
        var count = 0;
        var result = await _controller.GetPosts(domain, count);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Количество постов должно быть больше 0.", badRequestResult.Value);
    }
}