using MediatR;
using System;

namespace Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public record UpdateTaskStatusCommand(Guid Id, Domain.Enums.TaskStatus NewStatus) : IRequest<Application.Common.Models.Result<bool>>;
}
