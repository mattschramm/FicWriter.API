using FluentValidation;

namespace FicWriter.API.Shared.Draft;

public class DraftValidatorValidator : AbstractValidator<DraftRequest>
{
    public DraftValidatorValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(256)
            .WithMessage("Title must be at most 256 characters long");
    }
}
