using testudv.Domain.Entities;

namespace testudv.Domain.Interfaces;

public interface IPostInfoRepository
{
    Task SaveAnalysisAsync(PostInfo postInfo);
    Task<List<PostInfo>> GetPostsAsync();
}