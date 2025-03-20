namespace testudv.Application.Services;

public interface IPostService
{
    Task<string> GetPostsAsync(string domain, int count);
}