using MediatR;
using testudv.Application.Dtos;

namespace testudv.Application.Commands;

public class GetPostsCommand : IRequest<string>
{
    public int Count { get; set; }
    public string Domain {get; set;}
}