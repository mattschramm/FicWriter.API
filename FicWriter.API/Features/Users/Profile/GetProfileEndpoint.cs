using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Users.Profile;

public class GetProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/user", Handle)
            .RequireAuthorization()
            .WithName("GetUserProfile")
            .WithTags("Users");
    }

    public async Task<IResult> Handle(IMediator mediator)
    {
        var result = await mediator.Send(new GetProfileCommand());

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
