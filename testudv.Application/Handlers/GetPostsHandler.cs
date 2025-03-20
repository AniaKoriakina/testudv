using MediatR;
using testudv.Application.Commands;
using testudv.Application.Dtos;
using testudv.Application.Services;
using testudv.Domain.Entities;

namespace testudv.Application.Handlers;

public class GetPostsHandler : IRequestHandler<GetPostsCommand, string>
{
    private readonly PostService _postService;

    public GetPostsHandler(PostService postService)
    {
        _postService = postService;
    }

    public async Task<string> Handle(GetPostsCommand request, CancellationToken cancellationToken)
    {
        return await _postService.GetPostsAsync(request.Domain,request.Count);
    }
}
