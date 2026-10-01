using Application.Features.Tasks.Commands.CreateTask;
using Application.Features.Tasks.Commands.DeleteTask;
using Application.Features.Tasks.Commands.UpdateTask;
using Application.Features.Tasks.Queries.GetTaskById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ISender _sender;

        public TasksController(ISender sender)
        {
            _sender = sender;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskCommand command)
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
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _sender.Send(new GetTaskByIdQuery(Id));
            if(result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> Update(Guid Id,UpdateTaskCommand command)
        {
            if(Id != command.Id) return BadRequest("ID Mismatch");
            var result = await _sender.Send(command);
            if (!result.IsSuccess)
            {
                if (result.StatusCode.HasValue) return StatusCode(result.StatusCode.Value, new ProblemDetails { Title = result.Error, Status = result.StatusCode });
                return BadRequest(result.Error);
            }
            return NoContent();
        }
        [HttpPatch("{Id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid Id, Application.Features.Tasks.Commands.UpdateTaskStatus.UpdateTaskStatusCommand command)
        {
            if (Id != command.Id) return BadRequest("ID Mismatch");
            try
            {
                var result = await _sender.Send(command);
                if (!result.IsSuccess)
                {
                    if (result.StatusCode.HasValue) return StatusCode(result.StatusCode.Value, new ProblemDetails { Title = result.Error, Status = result.StatusCode });
                    return BadRequest(result.Error);
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _sender.Send(new DeleteTaskCommand(Id));
            if (!result.IsSuccess)
            {
                if (result.StatusCode.HasValue) return StatusCode(result.StatusCode.Value, new ProblemDetails { Title = result.Error, Status = result.StatusCode });
                return BadRequest(result.Error);
            }
            return NoContent();
        }
    }
}
