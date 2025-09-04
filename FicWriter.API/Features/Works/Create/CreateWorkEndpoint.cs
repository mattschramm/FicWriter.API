using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;

namespace FicWriter.API.Features.Works.Create;

[GroupName(EndpointGroupNames.WorksGeneral)]
public class CreateWorkEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPost("/", Handle)
            .WithName("CreateWork")
            .Produces<CreateWorkResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private async Task<IResult> Handle(
        CreateWorkCommand request,
        IMediator mediator,
        IValidator<CreateWorkCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(request);

        return result.Match(
            result => Results.CreatedAtRoute("GetWorkById", new { workId = result.Id }, result),
            errors => result.ToProblem());
    }
}
