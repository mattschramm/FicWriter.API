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
        app.MapDelete("/{id}", (string id, IMediator mediator, SqidsEncoder<long> encoder) =>
        {
            var decryptedId = encoder.Decode(id).Single();

            var result = Handle(decryptedId, mediator);
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
