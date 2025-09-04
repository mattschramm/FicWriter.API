using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Drafts.List;

[GroupName(EndpointGroupNames.Drafts)]
public class ListDraftsEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapGet("/", async (
            WorkId workId,
            IMediator mediator) =>
        {
            var result = await Handle(workId.Value, mediator);
            return result;
        })
            .Produces<ListDraftsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("ListDrafts")
            .WithDisplayName("List Drafts")
            .WithDescription("Lists all drafts for a work.");
    }

    private static async Task<IResult> Handle(long workId, IMediator mediator)
    {
        var command = new ListDraftsCommand(workId);

        var result = await mediator.Send(command);

        return result.Value.Total == 0
            ? Results.NoContent()
            : result.Match(
            response => Results.Ok(response),
            error => result.ToProblem());
    }
}
