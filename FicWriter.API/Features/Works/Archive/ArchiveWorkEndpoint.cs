using FicWriter.API.Binders;
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
        app.MapPatch("/", async (
                WorkId workId,
                IMediator mediator) =>
            {
                var result = await Handle(workId.Value, mediator);
                return result;
            })
            .WithName("ArchiveWork")
            .WithDisplayName("Archive Work")
            .WithDescription("Archive or unarchive a work by its ID.")
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
