using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sqids;

namespace FicWriter.API.Features.Works.Archive;

[GroupName(EndpointGroupNames.Works)]
public class ArchiveWorkEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPatch("/{id}", async (
                [FromRoute] string id,
                IMediator mediator,
                SqidsEncoder<long> encoder) =>
            {
                var decryptedId = encoder.Decode(id).Single();

                var result = await Handle(decryptedId, mediator);
                return result;
            })
            .WithName("ArchiveWork")
            .Produces<ArchiveWorkResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Handle(long id, IMediator mediator)
    {
        var command = new ArchiveWorkCommand(id);
        
        var result = await mediator.Send(command);
        
        return result.Match(
            result => Results.Ok(result),
            error => result.ToProblem());
    }
}
