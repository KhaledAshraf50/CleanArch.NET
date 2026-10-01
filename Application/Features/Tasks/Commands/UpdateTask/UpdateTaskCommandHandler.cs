using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Application.Common.Models.Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Application.Common.Models.Result<bool>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);

            if (task == null) return Application.Common.Models.Result<bool>.Failure("Task not found", 404);

            task.Title = request.Title;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
            return Application.Common.Models.Result<bool>.Success(true);
        }
    }
}
