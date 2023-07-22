using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContextFactory _databaseContextFactory;

    public PostRepository(DatabaseContextFactory databaseContextFactory)
    {
        _databaseContextFactory = databaseContextFactory;
    }

    public async Task CreateAsync(PostEntity post)
    {
        using var context = _databaseContextFactory.CreateDbContext();
        context.Posts.Add(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        using var context = _databaseContextFactory.CreateDbContext();
        var post = await GetByIdAsync(postId);
        if (post == null)
            return;

        context.Posts.Remove(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task<PostEntity> GetByIdAsync(Guid postId)
    {
        using var context = _databaseContextFactory.CreateDbContext();
        return await context.Posts
            .Include(post => post.Comments)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PostEntity>> ListAllAsync()
    {
        using var context = _databaseContextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(post => post.Comments).AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        using var context = _databaseContextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(post => post.Comments).AsNoTracking()
            .Where(post => post.Author != null && post.Author.Contains(author))
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithCommentsAsync()
    {
        using var context = _databaseContextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(post => post.Comments).AsNoTracking()
            .Where(post => post.Comments != null && post.Comments.Any())
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    {
        using var context = _databaseContextFactory.CreateDbContext();
        return await context.Posts.AsNoTracking()
            .Include(post => post.Comments).AsNoTracking()
            .Where(post => post.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        using var context = _databaseContextFactory.CreateDbContext();
        context.Posts.Update(post);
        _ = await context.SaveChangesAsync();
    }
}