using FicWriter.API.Endpoints;
using FicWriter.API.Features.Users.Common;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;

namespace FicWriter.API.Features.Users.Create;

[GroupName(EndpointGroupNames.Users)]
public class CreateUserEndpoint : IEndpoint
{ 
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPost("/", Handle)
            .WithName("CreateUser")
            .AllowAnonymous()
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);
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
