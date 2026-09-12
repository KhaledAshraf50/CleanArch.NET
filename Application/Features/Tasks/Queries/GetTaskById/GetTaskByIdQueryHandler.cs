using Application.Common.Interfaces;
using Application.Features.Tasks.Commands.CreateTask;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTaskByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);
            if(task == null) return null;

            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                Title = task.Title,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                Status = task.Status.ToString()
            };
        }
    }
}
