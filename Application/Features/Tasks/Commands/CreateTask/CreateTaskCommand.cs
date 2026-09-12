using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public record CreateTaskCommand(string Title, string? Description,DateTime? DueDate,Guid ProjectId) : IRequest<TaskDto>;

}
