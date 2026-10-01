using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Domain.Entities.Task;
namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Application.Common.Interfaces.ICurrentUserService _currentUser;

        public CreateTaskCommandHandler(IUnitOfWork unitOfWork, Application.Common.Interfaces.ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Task must belong to an existing (not deleted) Project
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                throw new InvalidOperationException("Cannot create a task for a project that does not exist or was deleted.");
            }

            var Task = new Task
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                ProjectId = request.ProjectId,
                Status = Domain.Enums.TaskStatus.Todo,

            };

            await _unitOfWork.Tasks.AddAsync(Task);
            await _unitOfWork.SaveChangesAsync();
            return new TaskDto
            {
                Id = Task.Id,
                Title = Task.Title,
                Description = Task.Description,
                Status = Task.Status.ToString(),
                DueDate = Task.DueDate,
                ProjectId = Task.ProjectId,
                CreatedAt = Task.CreatedAt,
            };
        }
    }
}
