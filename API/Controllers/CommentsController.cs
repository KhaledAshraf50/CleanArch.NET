using Application.Features.Comments.Commands.AddComment;
using Application.Features.Comments.Commands.DeleteComment;
using Application.Features.Comments.Queries.GetTaskComments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ISender _sender;
        public CommentsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCommentCommand command)
        {
            try
            {
                var result = await _sender.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("task/{taskId:guid}")]
        public async Task<IActionResult> GetByTask(Guid taskId)
        {
            var result = await _sender.Send(new GetTaskCommentsQuery(taskId));
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _sender.Send(new DeleteCommentCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
