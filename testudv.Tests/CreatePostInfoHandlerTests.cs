using Microsoft.Extensions.Logging;
using Moq;
using testudv.Application.Commands;
using testudv.Application.Handlers;
using testudv.Application.Services;
using testudv.Domain.Interfaces;

namespace testudv.Tests;

public class CreatePostInfoHandlerTests
{
    private readonly Mock<IPostInfoRepository> _postInfoRepositoryMock;
    private readonly Mock<IPostService> _postServiceMock; 
    private readonly Mock<ILogger<CreatePostInfoHandler>> _loggerMock;
    private readonly CreatePostInfoHandler _handler;

    public CreatePostInfoHandlerTests()
    {
        _postInfoRepositoryMock = new Mock<IPostInfoRepository>();
        _postServiceMock = new Mock<IPostService>(); 
        _loggerMock = new Mock<ILogger<CreatePostInfoHandler>>();

        _handler = new CreatePostInfoHandler(
            _postInfoRepositoryMock.Object,
            _postServiceMock.Object, 
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidText_ReturnsCorrectLetterCounts()
    {
        var domain = "test_domain";
        var count = 5;
        var text = "абвгдабббвв";
        var command = new CreatePostInfoCommand { Domain = domain, Count = count };

        _postServiceMock.Setup(service => service.GetPostsAsync(domain, count))
            .ReturnsAsync(text);
        
        var result = await _handler.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(domain, result.Domain);
        Assert.Equal(count, result.Count);

        var expectedLetterCounts = new Dictionary<char, int>
        {
            { 'а', 2 },
            { 'б', 4 },
            { 'в', 3 },
            { 'г', 1 },
            { 'д', 1 }
        };

        foreach (var kvp in expectedLetterCounts)
        {
            Assert.True(result.LettersCount.ContainsKey(kvp.Key));
            Assert.Equal(kvp.Value, result.LettersCount[kvp.Key]);
        }
    }
}