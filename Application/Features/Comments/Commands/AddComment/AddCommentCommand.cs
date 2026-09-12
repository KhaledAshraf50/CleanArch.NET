using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.AddComment
{
    public record AddCommentCommand(string Content, Guid TaskId) : IRequest<CommentDto>;
 
}
