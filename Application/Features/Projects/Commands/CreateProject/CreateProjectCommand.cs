using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.CreateProject
{
    public record CreateProjectCommand(string Name, string? Description) : IRequest<ProjectDto>;
}
