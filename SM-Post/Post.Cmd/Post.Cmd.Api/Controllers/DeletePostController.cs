using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DeletePostController : ControllerBase
{
    private readonly ILogger<DeletePostController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public DeletePostController(ILogger<DeletePostController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher; 
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> EditMessageAsync(Guid id, DeletePostCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Delete post request completed successfully!"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Client made a bad request!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.LogWarning(ex, "Could not retrieve aggregate, client passed an incorrect post ID targeting the aggregate!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string SafeErrorMessage = "Error while processing request to delete a post!";
            _logger.LogError(ex, SafeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SafeErrorMessage
            });
        }
    }
}