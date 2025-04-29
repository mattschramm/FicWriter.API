using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;

namespace FicWriter.API.Features.Works.Create;

public record CreateWorkRequest(string Title, string Description);

public class CreateWorkEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/works", Handle)
            .WithName("CreateWork")
            .RequireAuthorization()
            .Produces<CreateWorkResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithTags("Works");
    }

    private async Task<IResult> Handle(
        CreateWorkRequest request,
        IMediator mediator,
        IValidator<CreateWorkRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(request.ToCommand());

        /*return result.Match(
            result => Results.CreatedAtRoute("GetWorkById", new { id = result.Id }, result),
            errors => result.ToProblem());*/

        return result.Match(
            result => Results.Created(string.Empty, result),
            errors => result.ToProblem());
    }
}
