using Application.Features.Tasks.Commands.CreateTask;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Queries.GetTasks
{
    public record GetTasksQuery() : IRequest<List<TaskDto>>;

}
