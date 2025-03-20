using MediatR;
using Microsoft.Extensions.Logging;
using testudv.Application.Commands;
using testudv.Application.Dtos;
using testudv.Application.Services;
using testudv.Domain.Entities;
using testudv.Domain.Interfaces;

namespace testudv.Application.Handlers;

public class CreatePostInfoHandler : IRequestHandler<CreatePostInfoCommand, PostInfoDto>
{
    private readonly IPostInfoRepository _postInfoRepository;
    private readonly ILogger<CreatePostInfoHandler> _logger;
    private readonly IPostService _postService;

    public CreatePostInfoHandler(
        IPostInfoRepository postInfoRepository, 
        IPostService postService, 
        ILogger<CreatePostInfoHandler> logger)
    {
        _postInfoRepository = postInfoRepository;
        _postService = postService;
        _logger = logger;
    }

    public async Task<PostInfoDto> Handle(CreatePostInfoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Начало выполнения команды для Domain={request.Domain}, Count={request.Count}");
        string text = await _postService.GetPostsAsync(request.Domain, request.Count);
        string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        int letterCount = 0;
        Dictionary<char, int> postInfoDict = new Dictionary<char, int>();
        
        _logger.LogInformation($"Начало подсчета букв...");
        foreach (char c in text)
        {
            if (alphabet.Contains(c))
            {
                letterCount++;
                postInfoDict[c] = postInfoDict.TryGetValue(c, out int count) ? count + 1 : 1;
            }
        }
        _logger.LogInformation($"Подсчет завершился успешно. Было обработано {letterCount} букв.");

        var postInfo = new PostInfo(
            request.Domain,
            request.Count,
            postInfoDict
        );
        await _postInfoRepository.SaveAnalysisAsync(postInfo);
        var sortedPostInfoDict = postInfoDict
            .OrderBy(c => alphabet.IndexOf(c.Key))
            .ToDictionary(c => c.Key, c => c.Value);
        return new PostInfoDto(
            request.Domain,
            request.Count,
            sortedPostInfoDict
        );
    }
}