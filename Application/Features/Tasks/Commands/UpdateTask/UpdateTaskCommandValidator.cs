using FluentValidation;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Task Title Is Required")
                .MaximumLength(150).WithMessage("Task Title must not exceed 150 characters");
        }
    }
}
