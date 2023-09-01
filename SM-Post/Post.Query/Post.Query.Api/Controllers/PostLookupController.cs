using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController : ControllerBase
{
    private readonly ILogger<PostLookupController> _logger;
    private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

    public PostLookupController(IQueryDispatcher<PostEntity> queryDispatcher, ILogger<PostLookupController> logger)
    {
        _queryDispatcher = queryDispatcher;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPostsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SafeErrorMessage = "Error while processing request to retrieve all posts!";
            return ErrorResponse(ex, SafeErrorMessage);
        }
    }

    [HttpGet("byId/{postId}")]
    public async Task<ActionResult> GetByPostIdAsync(Guid postId)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SafeErrorMessage = "Error while processing request to retrieve post by ID!";
            return ErrorResponse(ex, SafeErrorMessage);
        }
    }

    [HttpGet("byAuthor/{author}")]
    public async Task<ActionResult> GetByAuthorAsync(string author)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery { Author = author });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SafeErrorMessage = "Error while processing request to retrieve all posts by author!";
            return ErrorResponse(ex, SafeErrorMessage);
        }
    }

    [HttpGet("withComments")]
    public async Task<ActionResult> GetPostsWithCommentsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SafeErrorMessage = "Error while processing request to retrieve all posts by author!";
            return ErrorResponse(ex, SafeErrorMessage);
        }
    }

    [HttpGet("withLikes/{numberOfLikes}")]
    public async Task<ActionResult> GetPostsWithLikesAsync(int numberOfLikes)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SafeErrorMessage = "Error while processing request to retrieve all posts by author!";
            return ErrorResponse(ex, SafeErrorMessage);
        }
    }

    private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
    {
        _logger.LogError(ex, safeErrorMessage);

        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        {
            Message = safeErrorMessage
        });
    }

    private ActionResult NormalResponse(List<PostEntity> posts)
    {
        if (posts == null || !posts.Any())
            return NoContent();

        var count = posts.Count;

        return Ok(new PostLookupResponse
        {
            Posts = posts,
            Message = $"Successfully returns {count} post{(count > 1 ? "s" : string.Empty)}!"
        });
    }
}