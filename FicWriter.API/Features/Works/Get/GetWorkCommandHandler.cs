using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Get;

public record GetWorkCommand(long WorkId) : IRequest<ErrorOr<GetWorkResponse>>;

public record GetWorkResponse(string WorkId, string Title, string Description);

public class GetWorkCommandHandler : IRequestHandler<GetWorkCommand, ErrorOr<GetWorkResponse>>
{
    private readonly IWorkReadOnly _workReadOnly;
    private readonly ICurrentUser _currentUser;

    public GetWorkCommandHandler(IWorkReadOnly workReadOnly, ICurrentUser currentUser)
    {
        _workReadOnly = workReadOnly;
        _currentUser = currentUser;
    }

    public async Task<ErrorOr<GetWorkResponse>> Handle(GetWorkCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        var work = await _workReadOnly.GetById(currentUser, request.WorkId);

        if (work is null)
            return Error.NotFound(description: "Work not found");

        //TEMP
        return new GetWorkResponse(
            WorkId: work.Id.ToString(),
            Title: work.Title,
            Description: work.Description);
    }
}
