using Application.Features.Tasks.Commands.CreateTask;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public record UpdateTaskCommand(
        Guid Id,
        string Title,
        string? Description,
        DateTime? DueDate
        ) : IRequest<Application.Common.Models.Result<bool>>;
   
}
