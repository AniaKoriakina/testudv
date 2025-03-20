using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using testudv.API.Controllers;
using testudv.Application.Commands;
using testudv.Application.Dtos;
using Xunit;

namespace testudv.Tests;

public class PostsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PostsController _controller;

    public PostsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PostsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetPosts_ValidDomainAndCount_ReturnsOkResult()
    {
        var domain = "test_domain";
        var count = 5;
        var expectedResult = "longtext";
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPostsCommand>(), default))
            .ReturnsAsync(expectedResult.Trim());
        var result = await _controller.GetPosts(domain, count);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsAssignableFrom<string>(okResult.Value);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task GetPosts_EmptyDomain_ReturnsBadRequest()
    {
        var mediatorMock = new Mock<IMediator>();
        var controller = new PostsController(mediatorMock.Object);
        var result = await controller.GetPosts(string.Empty, 5);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Domain cannot be empty.", badRequestResult.Value);
    }
}