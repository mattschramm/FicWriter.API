using FicWriter.API.Enums;
using FluentValidation;

namespace FicWriter.API.Features.Dashboard;

public class GetDashboardValidator : AbstractValidator<GetDashboardCommand>
{
    public GetDashboardValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 128)
            .WithMessage("Page size must be between 1 and 128");

        RuleForEach(x => x.Tags)
            .MaximumLength(64)
            .WithMessage("Tag length must be less than or equal to 64 characters")
            .NotEmpty()
            .WithMessage("Tag cannot be empty");
    }
}