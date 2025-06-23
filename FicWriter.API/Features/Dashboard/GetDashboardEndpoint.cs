using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Validator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Dashboard;

[GroupName(EndpointGroupNames.Dashboard)]
public class GetDashboardEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapGet("/", async (
            [AsParameters] GetDashboardCommand command,
            [FromServices] IMediator mediator,
            [FromServices] IValidator<GetDashboardCommand> validator) =>
        {
            var result = await Handle(command, mediator, validator);
            return result;
        })
            .ProducesValidationProblem()
            .Produces<GetDashboardResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .WithName("GetDashboard")
            .WithDescription("Get a paginated list of works for the dashboard.")
            .WithDisplayName("Dashboard");
    }

    private static async Task<IResult> Handle(GetDashboardCommand command, IMediator mediator, IValidator<GetDashboardCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(command);

        if (validationResult.IsNotValid())
        {
            return Results.ValidationProblem(errors: validationResult.ToDictionary());
        }

        var result = await mediator.Send(command);

        if (result.Value.Total == 0)
        {
            return Results.NoContent();
        }

        return result.Match(
            result => Results.Ok(result),
            error => result.ToProblem()
        );
    }
}
