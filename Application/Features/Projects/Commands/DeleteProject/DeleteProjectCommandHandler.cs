using Application.Common.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            if (project == null) return false;

            var userId = _currentUser?.UserId;
            if (userId == null) return false;
            if (project.OwnerId != userId && !_currentUser.IsInRole("Admin")) return false;

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
