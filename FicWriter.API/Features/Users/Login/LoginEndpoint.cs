using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FicWriter.API.Shared.User;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Users.Login;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/login", Handle)
            .WithName("LoginUser")
            .AllowAnonymous()
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithTags("Users");
    }

    public async Task<IResult> Handle(LoginCommand request, IMediator mediator, IValidator<LoginCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await mediator.Send(request);

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
