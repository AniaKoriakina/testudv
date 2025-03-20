using Microsoft.EntityFrameworkCore;
using testudv.Domain.Entities;
using testudv.Domain.Interfaces;
using testudv.Infrastructure.Data;

namespace testudv.Infrastructure.Repositories;

public class PostInfoRepository : IPostInfoRepository
{
    private readonly ApplicationDbContext _context;

    public PostInfoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveAnalysisAsync(PostInfo postInfo)
    {
        _context.PostsInfo.Add(postInfo);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PostInfo>> GetPostsAsync()
    {
        return await _context.PostsInfo.ToListAsync();
    }
}