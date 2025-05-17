using FicWriter.API.Shared.Mapper;
using FluentValidation;

namespace FicWriter.API.Features.Drafts.Create;

public class CreateDraftValidator : AbstractValidator<CreateDraftRequest>
{
    public CreateDraftValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(256)
            .WithMessage("Title must not exceed 256 characters.");
    }
}
