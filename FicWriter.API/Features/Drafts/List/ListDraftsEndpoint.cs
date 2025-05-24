using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.IdEncoder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sqids;

namespace FicWriter.API.Features.Drafts.List;

[GroupName(EndpointGroupNames.Drafts)]
public class ListDraftsEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapGet("/", async (
            [FromRoute] string workId,
            IMediator mediator,
            SqidsEncoder<long> encoder) =>
        {
            var decryptedId = encoder.DecodeSingle(workId);

            var result = await Handle(decryptedId, mediator);
            return result;
        })
            .Produces<ListDraftsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("ListDrafts");
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
