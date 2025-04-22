using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Shared.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Users.Auth;

public record AuthenticateUserRequest(string RefreshToken);

public class AuthenticateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/authenticate", Handle)
            .WithName("AuthenticateUser")
            .AllowAnonymous()
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithTags("Users");
    }

    private async Task<IResult> Handle([FromBody] AuthenticateUserRequest request, IMediator mediator)
    {
        var result = await mediator.Send(request.ToCommand());

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
