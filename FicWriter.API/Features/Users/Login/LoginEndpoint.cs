using FicWriter.API.Endpoints;
using FicWriter.API.Features.Users.Shared;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Users.Login;

public record LoginRequest(string Email, string Password);

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", Handle)
            .WithName("LoginUser")
            .AllowAnonymous()
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithTags("Users");
    }

    public async Task<IResult> Handle([FromBody] LoginRequest request, IMediator mediator, IValidator<LoginRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await mediator.Send(request.ToCommand());

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
