using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;
using Sqids;

namespace FicWriter.API.Features.Works.Delete;

[GroupName(EndpointGroupNames.Works)]
public class DeleteWorkEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapDelete("/", async (
            WorkId workId,
            IMediator mediator) =>
        {
            var result = await Handle(workId.Value, mediator);
            return result;
        })
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("DeleteWork");
    }

    private static async Task<IResult> Handle(long id, IMediator mediator)
    {
        var result = await mediator.Send(new DeleteWorkCommand(id));

        return result.Match(
            result => Results.NoContent(),
            error => result.ToProblem());
    }
}
