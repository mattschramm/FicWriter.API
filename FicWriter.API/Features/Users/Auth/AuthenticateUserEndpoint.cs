using FicWriter.API.Endpoints;
using FicWriter.API.Features.Users.Common;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Users.Auth;

[GroupName(EndpointGroupNames.Users)]
public class AuthenticateUserEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPost("/auth", Handle)
            .WithName("AuthenticateUser")
            .WithTags("Tokens")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    private async Task<IResult> Handle(AuthenticateUserCommand request, IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
