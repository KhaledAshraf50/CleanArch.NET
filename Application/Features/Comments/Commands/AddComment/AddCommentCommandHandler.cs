using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.AddComment
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddCommentCommandHandler(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            //  Comment must belong to an existing (not deleted) Task
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
            if (task == null)
            {
                throw new InvalidOperationException("Cannot add a comment to a task that does not exist or was deleted.");
            }

            var comment = new Comment
            {
                Content = request.Content,
                TaskId = request.TaskId,
            };

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                TaskId = comment.TaskId,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
