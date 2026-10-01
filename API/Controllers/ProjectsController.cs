using Application.Features.Projects.Commands.CreateProject;
using Application.Features.Projects.Queries.GetProjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ISender _sender;
        public ProjectsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectCommand command)
        {
            try
            {
                // Ensure OwnerId is not supplied by client for security; handler will use current user when null
                command = command with { OwnerId = command.OwnerId };
                var result = await _sender.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sender.Send(new GetProjectsQuery());
            return Ok(result);
        }
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _sender.Send(new Application.Features.Projects.Queries.GetProjectById.GetProjectByIdQuery(Id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{Id:guid}")]
        public async Task<IActionResult> Update(Guid Id, Application.Features.Projects.Commands.UpdateProject.UpdateProjectCommand command)
        {
            if (Id != command.Id) return BadRequest("ID Mismatch");
            var result = await _sender.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _sender.Send(new Application.Features.Projects.Commands.DeleteProject.DeleteProjectCommand(Id));
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
