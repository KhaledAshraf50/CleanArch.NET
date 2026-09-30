using MediatR;
using System;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public record UpdateProjectCommand(Guid Id, string Name, string? Description) : IRequest<bool>;
}
