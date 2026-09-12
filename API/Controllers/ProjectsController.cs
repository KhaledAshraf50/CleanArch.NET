using Application.Features.Projects.Commands.CreateProject;
using Application.Features.Projects.Queries.GetProjects;
using MediatR;
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
        

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectCommand command)
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sender.Send(new GetProjectsQuery());
            return Ok(result);
        }
    }
}
