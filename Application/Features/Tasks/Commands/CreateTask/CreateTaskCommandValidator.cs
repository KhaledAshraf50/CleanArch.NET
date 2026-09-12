using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator:AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Task Title Is Required")
                .MaximumLength(150).WithMessage("Task Title must not exceed 150 characters");

            RuleFor(t => t.ProjectId)
                .NotEmpty().WithMessage("ProjectId Is Required");
            
        }
    }
}
