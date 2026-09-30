using FluentValidation;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is Required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        }
    }
}
