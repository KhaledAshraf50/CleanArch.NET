using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Application.Common.Models.Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;            
        }
        public async Task<Application.Common.Models.Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);

            if (task == null) return Application.Common.Models.Result<bool>.Failure("Task not found", 404);

            _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.SaveChangesAsync();
            return Application.Common.Models.Result<bool>.Success(true);
        }
    }
}
