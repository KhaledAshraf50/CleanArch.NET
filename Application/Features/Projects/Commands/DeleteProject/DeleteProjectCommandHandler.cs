using Application.Common.Interfaces;
using MediatR;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Application.Common.Models.Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            if (project == null) return Result<bool>.Failure("Project not found", 404);

            var userId = _currentUser?.UserId;
            if (userId == null) return Result<bool>.Failure("Unauthorized", 401);
            if (project.OwnerId != userId && !_currentUser.IsInRole("Admin")) return Result<bool>.Failure("Forbidden", 403);

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
