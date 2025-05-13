using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sqids;

namespace FicWriter.API.Features.Works.Archive;

public class ArchiveWorkEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/work/{id}", async (
                [FromRoute] string id,
                IMediator mediator,
                SqidsEncoder<long> encoder) =>
            {
                var decryptedId = encoder.Decode(id).Single();

                var result = await Handle(decryptedId, mediator);
                return result;
            })
            .WithName("ArchiveWork")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Works");
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
