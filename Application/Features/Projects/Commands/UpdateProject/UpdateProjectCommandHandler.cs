using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Application.Common.Models.Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Application.Common.Models.Result<bool>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            if (project == null) return Application.Common.Models.Result<bool>.Failure("Project not found", 404);

            // only owner or Admin can update
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId)) return Application.Common.Models.Result<bool>.Failure("Unauthorized", 401);
            if (project.OwnerId != userId && !_currentUser.IsInRole("Admin")) return Application.Common.Models.Result<bool>.Failure("Forbidden", 403);

            project.Name = request.Name;
            project.Description = request.Description;
            project.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return Application.Common.Models.Result<bool>.Success(true);
        }
    }
}
