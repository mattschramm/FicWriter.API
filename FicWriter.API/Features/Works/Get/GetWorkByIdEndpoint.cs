using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;
using Sqids;

namespace FicWriter.API.Features.Works.Get;

public class GetWorkByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/works/{id}", async (
            string id,
            SqidsEncoder<long> encoder,
            IMediator mediator) =>
        {
            var decryptedId = encoder.Decode(id).Single();

            var result = await Handle(decryptedId, mediator);
            return result;
        })
            .RequireAuthorization()
            .Produces<GetWorkByIdResponse>(StatusCodes.Status200OK)
            .WithName("GetWorkById")
            .WithTags("Works");
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
