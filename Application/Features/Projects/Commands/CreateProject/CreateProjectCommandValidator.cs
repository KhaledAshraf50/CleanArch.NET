using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is Required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        }
    }
}
