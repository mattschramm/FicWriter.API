using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Enums;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Works.Update;

public record UpdateWorkRequest(string Title, string Description, List<Genres> Genres, List<string> Tags);

[GroupName(EndpointGroupNames.Works)]
public class UpdateWorkEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPut("/", async (
            WorkId workId,
            [FromBody] UpdateWorkRequest request,
            IMediator mediator,
            IValidator<UpdateWorkRequest> validator) =>
        {
            var result = await Handle(request, workId.Value, mediator, validator);
            return result;
        })
            .WithName("UpdateWork")
            .WithDescription("Update an existing work")
            .WithDisplayName("Update Work")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Handle(UpdateWorkRequest request, long id, IMediator mediator, IValidator<UpdateWorkRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var command = new UpdateWorkCommand(id, request.Title, request.Description, request.Genres, request.Tags);

        var result = await mediator.Send(command);

        return result.Match(
            result => Results.NoContent(),
            error => result.ToProblem()
        );
    }
}
