using ErrorOr;
using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;
using Sqids;

namespace FicWriter.API.Features.Works.Delete;

public class DeleteWorkEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/works/{id}", (string id, IMediator mediator, SqidsEncoder<long> encoder) =>
        {
            var decryptedId = encoder.Decode(id).Single();

            var result = Handle(decryptedId, mediator);
            return result;
        })
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("DeleteWork")
            .WithTags("Works");
    }

    private static async Task<IResult> Handle(long id, IMediator mediator)
    {
        var result = await mediator.Send(new DeleteWorkCommand(id));

        return result.Match(
            result => Results.NoContent(),
            error => result.ToProblem());
    }
}
