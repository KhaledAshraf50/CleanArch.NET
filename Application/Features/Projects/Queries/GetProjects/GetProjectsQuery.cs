using Application.Features.Projects.Commands.CreateProject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Queries.GetProjects
{
    public record GetProjectsQuery() : IRequest<List<ProjectDto>>;
}
