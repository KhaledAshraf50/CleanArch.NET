using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            if (project == null) return false;

            project.Name = request.Name;
            project.Description = request.Description;
            project.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
