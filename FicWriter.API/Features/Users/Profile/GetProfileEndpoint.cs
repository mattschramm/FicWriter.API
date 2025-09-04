using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Users.Profile;

[GroupName(EndpointGroupNames.Users)]
public class GetProfileEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapGet("/", Handle)
            .RequireAuthorization()
            .WithName("GetUserProfile");
    }

    public async Task<IResult> Handle(IMediator mediator)
    {
        var result = await mediator.Send(new GetProfileCommand());

        return result.Match(
            result => Results.Ok(result),
            errors => result.ToProblem());
    }
}
