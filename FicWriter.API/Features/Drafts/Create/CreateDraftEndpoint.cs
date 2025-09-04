using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Drafts.Create;

public record CreateDraftRequest(string Title);

[GroupName(EndpointGroupNames.Drafts)]
public class CreateDraftEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPost("/", async (
            WorkId workId,
            [FromBody] CreateDraftRequest request,
            IMediator mediator,
            IValidator<CreateDraftRequest> validator) =>
        {
            var result = await Handle(workId.Value, request, mediator, validator);
            return result;

        })
            .Produces<CreateDraftResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("CreateDraft")
            .WithDisplayName("Create Draft")
            .WithDescription("Creates a new draft for a work.");
    }

    private static async Task<IResult> Handle(long workId, CreateDraftRequest request, IMediator mediator, IValidator<CreateDraftRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var command = new CreateDraftCommand(request.Title, workId);
        
        var result = await mediator.Send(command);
        
        return result.Match(
            draft => Results.Created(string.Empty, draft),
            error => result.ToProblem());
    }
}
