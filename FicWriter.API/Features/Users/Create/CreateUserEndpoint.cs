using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FicWriter.API.Shared.User;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Users.Create;

public sealed record CreateUserRequest(string Name, string Email, string Password);

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

    private async Task<IResult> Handle([FromBody] CreateUserRequest request, IMediator mediator, IValidator<CreateUserRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(request.ToCommand());

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
