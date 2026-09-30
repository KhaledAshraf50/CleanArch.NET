using Application.Common.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(request.Id);
            if (comment == null) return false;

            _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
