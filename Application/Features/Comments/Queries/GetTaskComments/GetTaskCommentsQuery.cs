using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Features.Comments.Queries.GetTaskComments
{
    public record GetTaskCommentsQuery(Guid TaskId) : IRequest<List<Application.Features.Comments.Commands.AddComment.CommentDto>>;
}
