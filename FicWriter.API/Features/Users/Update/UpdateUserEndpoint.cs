using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;

namespace FicWriter.API.Features.Users.Update;

public record UpdateUserRequest(string Name, string Email);

public class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/user", Handle)
            .RequireAuthorization()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status204NoContent)
            .WithName("UpdateUser")
            .WithTags("Users");
    }

    private async Task<IResult> Handle(UpdateUserRequest request, IMediator mediator, IValidator<UpdateUserRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(request.ToCommand());

        return result.Match(
            result => Results.NoContent(),
            errors => result.ToProblem());
    }
}
