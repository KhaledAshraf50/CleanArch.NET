using Application.Common.Interfaces;
using Application.Features.Tasks.Commands.CreateTask;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TaskDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTasksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public async Task<List<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();

            return tasks.Select(t=>new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description= t.Description,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                Status = t.Status.ToString(),
                ProjectId = t.ProjectId,
            }).ToList();
        }
    }
}
