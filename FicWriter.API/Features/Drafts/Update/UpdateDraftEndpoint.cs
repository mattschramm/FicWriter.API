using FicWriter.API.Endpoints;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.IdEncoder;
using FicWriter.API.Shared.Draft;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sqids;

namespace FicWriter.API.Features.Drafts.Update;

[GroupName(EndpointGroupNames.Drafts)]
public class UpdateDraftEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MapPut("/{draftId:long}", async (
            [FromRoute] string workId,
            [FromRoute] long draftId,
            [FromBody] DraftRequest request,
            IMediator mediator,
            IValidator<DraftRequest> validator,
            SqidsEncoder<long> encoder) =>
        {
            var decryptedWorkId = encoder.DecodeSingle(workId);
            var result = await Handle(request, decryptedWorkId, draftId, mediator, validator);
            return result;
        });
    }

    private static async Task<IResult> Handle(DraftRequest request, long workId, long draftId, IMediator mediator, IValidator<DraftRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var command = new UpdateDraftCommand(workId, draftId, request.Title);

        var result = await mediator.Send(command);

        return result.Match(
            _ => Results.NoContent(),
            error => result.ToProblem());
    }
}
