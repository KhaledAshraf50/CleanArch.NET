using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);
            if (task == null) return false;

            var current = task.Status;
            var next = request.NewStatus;

            if (current == next) return true; // no-opertion

            // Allowed transitions:
            // Todo -> InProgress
            // InProgress -> Completed
            // Todo -> Cancelled
            // InProgress -> Cancelled

            var allowed = false;
            if (current == Domain.Enums.TaskStatus.Todo)
            {
                if (next == Domain.Enums.TaskStatus.InProgress || next == Domain.Enums.TaskStatus.Cancelled) allowed = true;
            }
            else if (current == Domain.Enums.TaskStatus.InProgress)
            {
                if (next == Domain.Enums.TaskStatus.Completed || next == Domain.Enums.TaskStatus.Cancelled) allowed = true;
            }
            else
            {
                // current is Completed or Cancelled -> no further transitions allowed
                allowed = false;
            }

            if (!allowed)
            {
                throw new InvalidOperationException($"Invalid status transition from {current} to {next}.");
            }

            task.Status = next;
            task.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
