using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Users.Revoke;

public class RevokeRefreshTokenEndpoint() : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("users/{id:guid}/refresh-token", async (Guid id, [FromServices] IMediator mediator) =>
        {
            var result = await Handle(id, mediator);
            return result;
        })
            .RequireAuthorization("SameUser")
            .WithName("RevokeRefreshToken")
            .WithTags("Users");
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
