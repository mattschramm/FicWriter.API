using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Users.Revoke;

[GroupName(EndpointGroupNames.Users)]
public class RevokeRefreshTokenEndpoint() : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapDelete("/{id:guid}/refresh-token", async (Guid id, IMediator mediator) =>
        {
            var result = await Handle(id, mediator);
            return result;
        })
            .RequireAuthorization("SameUser")
            .WithTags("Tokens")
            .WithName("RevokeRefreshToken");
    }

    public async Task<IResult> Handle(Guid guid, IMediator mediator)
    {
        var command = new RevokeRefreshTokenCommand(guid);
        var result = await mediator.Send(command);

        return result.Match(
            _ => Results.NoContent(),
            errors => result.ToProblem());
    }
}
