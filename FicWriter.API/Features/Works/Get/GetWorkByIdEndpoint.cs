using FicWriter.API.Binders;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Works.Get;

[GroupName(EndpointGroupNames.Works)]
public class GetWorkByIdEndpoint : IEndpoint
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
            .RequireAuthorization()
            .Produces<GetWorkByIdResponse>(StatusCodes.Status200OK)
            .WithName("GetWorkById");
    }

    private static async Task<IResult> Handle(long id, IMediator mediator)
    {
        var command = new GetWorkByIdCommand(id);

        var result = await mediator.Send(command);

        return result.Match(
            work => Results.Ok(work),
            error => result.ToProblem());
    }
}
