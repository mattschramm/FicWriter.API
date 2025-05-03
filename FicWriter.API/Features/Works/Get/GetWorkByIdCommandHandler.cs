using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using FicWriter.API.Models;
using MediatR;

namespace FicWriter.API.Features.Works.Get;

public record GetWorkByIdCommand(long Id) : IRequest<ErrorOr<GetWorkByIdResponse>>;

public record GetWorkByIdResponse(string Id, string Title, string Description, List<Draft> Drafts);

public class GetWorkByIdCommandHandler(IWorkReadOnly workReadOnly, ICurrentUser currentUser, GetWorkByIdMapper mapper) : IRequestHandler<GetWorkByIdCommand, ErrorOr<GetWorkByIdResponse>>
{
    private readonly IWorkReadOnly _workReadOnly = workReadOnly;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly GetWorkByIdMapper _mapper = mapper;

    public async Task<ErrorOr<GetWorkByIdResponse>> Handle(GetWorkByIdCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        var work = await _workReadOnly.GetById(currentUser, request.Id);

        if (work is null)
            return Error.NotFound(description: "Work not found");

        return _mapper.ToResponse(work);
    }
}
