using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using FicWriter.API.Models;
using MediatR;

namespace FicWriter.API.Features.Works.Get;

public record GetWorkCommand(long WorkId) : IRequest<ErrorOr<GetWorkResponse>>;

public record GetWorkResponse(string WorkId, string Title, string Description, List<Draft> Drafts);

public class GetWorkCommandHandler(IWorkReadOnly workReadOnly, ICurrentUser currentUser, GetWorkMapper mapper) : IRequestHandler<GetWorkCommand, ErrorOr<GetWorkResponse>>
{
    private readonly IWorkReadOnly _workReadOnly = workReadOnly;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly GetWorkMapper _mapper = mapper;

    public async Task<ErrorOr<GetWorkResponse>> Handle(GetWorkCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        var work = await _workReadOnly.GetById(currentUser, request.WorkId);

        if (work is null)
            return Error.NotFound(description: "Work not found");

        return _mapper.ToResponse(work);
    }
}
