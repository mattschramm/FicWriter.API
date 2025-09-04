using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Shared.Draft;
using MediatR;

namespace FicWriter.API.Features.Drafts.Get;

[GroupName(EndpointGroupNames.Drafts)]
public class GetDraftEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapGet("/{draftId:long}", async (
            WorkId workId,
            long draftId,
            IMediator mediator) =>
        {
            var result = await Handle(workId.Value, draftId, mediator);
            return result;
        })
            .WithName("GetDraft")
            .Produces<DraftResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Handle(long workId, long draftId, IMediator mediator)
    {
        var command = new GetDraftCommand(workId, draftId);

        var result = await mediator.Send(command);

        return result.Match(
            result => Results.Ok(result),
            error => result.ToProblem());
    }
}
