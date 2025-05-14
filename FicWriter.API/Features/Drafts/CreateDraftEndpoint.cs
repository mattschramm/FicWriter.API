using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sqids;

namespace FicWriter.API.Features.Drafts;

public record CreateDraftRequest(string Title, uint Order);

[EndpointGroup(EndpointGroupNames.Drafts)]
public class CreateDraftEndpoint : IGroupedEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPost("/", async (
            [FromRoute] string workId,
            [FromBody] CreateDraftRequest request,
            IMediator mediator,
            IValidator<CreateDraftRequest> validator,
            SqidsEncoder<long> encoder) =>
        {
            var decryptedWorkId = encoder.Decode(workId).Single();

            var result = await Handle(decryptedWorkId, request, mediator, validator);
            return result;

        })
            .Produces<CreateDraftResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("CreateDraft");
    }

    private static async Task<IResult> Handle(long workId, CreateDraftRequest request, IMediator mediator, IValidator<CreateDraftRequest> validator)
    {
        var command = new CreateDraftCommand(request.Title, request.Order, workId);
        
        var result = await mediator.Send(command);
        
        return result.Match(
            draft => Results.Created(string.Empty, draft),
            error => result.ToProblem());
    }
}
