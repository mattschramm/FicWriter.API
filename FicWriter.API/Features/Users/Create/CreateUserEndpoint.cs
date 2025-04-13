using ErrorOr;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Users.Create;

public class CreateUserEndpoint : IEndpoint
{ 
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", Handle)
            .WithName("CreateUser")
            .AllowAnonymous()
            .Produces<CreateUserResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
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
