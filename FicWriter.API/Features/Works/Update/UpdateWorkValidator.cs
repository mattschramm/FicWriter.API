using FluentValidation;

namespace FicWriter.API.Features.Works.Update;

public class UpdateWorkValidator : AbstractValidator<UpdateWorkRequest>
{
    public UpdateWorkValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(500)
            .WithMessage("Title must be at most 500 characters long.");
        
        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must be at most 2000 characters long.");

        RuleForEach(x => x.Genres)
            .IsInEnum()
            .WithMessage("Invalid genre.");

        RuleForEach(x => x.Tags)
            .MaximumLength(64)
            .WithMessage("Tag length must be less than or equal to 64 characters.")
            .NotEmpty()
            .WithMessage("Tag cannot be empty.");
    }
}
