using FicWriter.API.Endpoints;
using FicWriter.API.Enums;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sqids;

namespace FicWriter.API.Features.Works.Update;

public record UpdateWorkRequest(string Title, string Description, List<Genres> Genres, List<string> Tags);

[GroupName(EndpointGroupNames.Works)]
public class UpdateWorkEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPut("/{id}", async (
            [FromRoute] string id,
            [FromBody] UpdateWorkRequest request,
            IMediator mediator,
            IValidator<UpdateWorkRequest> validator,
            SqidsEncoder<long> encoder) =>
        {
            var decryptedId = encoder.Decode(id).Single();

            var result = await Handle(request, decryptedId, mediator, validator);
            return result;
        })
            .WithName("UpdateWork")
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
