using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;

namespace FicWriter.API.Features.Users.Update;

[GroupName(EndpointGroupNames.Users)]
public class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPut("/", Handle)
            .RequireAuthorization()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status204NoContent)
            .WithName("UpdateUser");
    }

    private async Task<IResult> Handle(UpdateUserCommand request, IMediator mediator, IValidator<UpdateUserCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(request);

        return result.Match(
            result => Results.NoContent(),
            errors => result.ToProblem());
    }
}
