using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands.AddComment
{
    public class AddCommentCommandValidator:AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Comment Content Cannot be Empty")
                .MaximumLength(500).WithMessage("Comment Must not exceed 500 character");

            RuleFor(c => c.TaskId)
                .NotEmpty().WithMessage("TaskId is Required");
        }
    }
}
