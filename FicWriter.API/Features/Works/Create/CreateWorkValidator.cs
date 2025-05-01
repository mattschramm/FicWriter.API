using FluentValidation;

namespace FicWriter.API.Features.Works.Create;

public class CreateWorkValidator : AbstractValidator<CreateWorkCommand>
{
    public CreateWorkValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(500)
            .WithMessage("Title must be at most 500 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(2000)
            .WithMessage("Description must be at most 2000 characters long.");
    }
}
