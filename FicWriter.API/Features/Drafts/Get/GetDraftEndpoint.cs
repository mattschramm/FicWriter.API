using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.IdEncoder;
using MediatR;
using Sqids;

namespace FicWriter.API.Features.Drafts.Get;

[GroupName(EndpointGroupNames.Drafts)]
public class GetDraftEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapGet("/{draftId:long}", async (
            string workId,
            long draftId,
            IMediator mediator,
            SqidsEncoder<long> encoder) =>
        {
            var decryptedWorkId = encoder.DecodeSingle(workId);

            var result = await Handle(decryptedWorkId, draftId, mediator);
            return result;
        })
            .WithName("GetDraft")
            .Produces<GetDraftResponse>(StatusCodes.Status200OK)
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
