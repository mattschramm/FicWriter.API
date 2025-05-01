using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Shared.User;
using MediatR;

namespace FicWriter.API.Features.Users.Auth;

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

    private async Task<IResult> Handle(AuthenticateUserCommand request, IMediator mediator)
    {
        var result = await mediator.Send(request);

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
