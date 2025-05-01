using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FicWriter.API.Shared.User;
using FluentValidation;
using MediatR;

namespace FicWriter.API.Features.Users.Create;

public class CreateUserEndpoint : IEndpoint
{ 
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/register", Handle)
            .WithName("CreateUser")
            .AllowAnonymous()
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags("Users");
    }

    private async Task<IResult> Handle(
        CreateUserCommand request,
        IMediator mediator,
        IValidator<CreateUserCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(request);

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
