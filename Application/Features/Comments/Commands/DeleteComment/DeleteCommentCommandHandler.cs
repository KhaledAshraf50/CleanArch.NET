using Application.Common.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Application.Common.Models.Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Application.Common.Models.Result<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(request.Id);
            if (comment == null) return Application.Common.Models.Result<bool>.Failure("Comment not found", 404);

            _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
            return Application.Common.Models.Result<bool>.Success(true);
        }
    }
}
