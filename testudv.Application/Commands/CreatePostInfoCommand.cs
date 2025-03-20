using MediatR;
using testudv.Application.Dtos;

namespace testudv.Application.Commands;

public class CreatePostInfoCommand : IRequest<PostInfoDto>
{
    public string Domain { get; set; }
    public int Count { get; set; }
}