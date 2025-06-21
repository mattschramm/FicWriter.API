using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Drafts.Delete;

[GroupName(EndpointGroupNames.Drafts)]
public class DeleteDraftEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapDelete("/{draftId:long}", async (
            WorkId workId,
            long draftId,
            IMediator mediator) =>
        {
            var result = await Handle(workId.Value, draftId, mediator);

            return result;
        })
            .WithName("DeleteDraft")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Handle(long WorkId, long DraftId, IMediator mediator)
    {
        var command = new DeleteDraftCommand(DraftId, WorkId);

        var result = await mediator.Send(command);

        return result.Match(
            _ => Results.NoContent(),
            error => result.ToProblem());
    }
}
