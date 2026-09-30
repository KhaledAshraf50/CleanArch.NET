using Application.Common.Interfaces;
using Application.Features.Comments.Commands.AddComment;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Comments.Queries.GetTaskComments
{
    public class GetTaskCommentsQueryHandler : IRequestHandler<GetTaskCommentsQuery, List<CommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTaskCommentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CommentDto>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _unitOfWork.Comments.FindAsync(c => c.TaskId == request.TaskId);

            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                TaskId = c.TaskId,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}
