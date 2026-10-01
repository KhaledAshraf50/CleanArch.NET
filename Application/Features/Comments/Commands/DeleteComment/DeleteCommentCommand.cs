using MediatR;
using System;

namespace Application.Features.Comments.Commands.DeleteComment
{
    public record DeleteCommentCommand(Guid Id) : IRequest<Application.Common.Models.Result<bool>>;
}
