using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            if (project == null) return false;

            // only owner or Admin can update
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId)) return false;
            if (project.OwnerId != userId && !_currentUser.IsInRole("Admin")) return false;

            project.Name = request.Name;
            project.Description = request.Description;
            project.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
