using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Mediator.Requests;
using FicWriter.API.Shared.Draft;
using MediatR;

namespace FicWriter.API.Features.Drafts.List;

public record ListDraftsCommand(long WorkId) : IWorkRequest<ErrorOr<ListDraftsResponse>>;

public record ListDraftsResponse(List<DraftResponse> Drafts, int Total);

public class ListDraftsCommandHandler(IDraftRepository draftRepository, ListDraftsMapper mapper) : IRequestHandler<ListDraftsCommand, ErrorOr<ListDraftsResponse>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly ListDraftsMapper _mapper = mapper;

    public async Task<ErrorOr<ListDraftsResponse>> Handle(ListDraftsCommand request, CancellationToken cancellationToken)
    {
        var drafts = await _draftRepository.GetDrafts(request.WorkId);

        if (drafts.Count == 0)
        {
            return new ListDraftsResponse([], 0);
        }

        return _mapper.ToResponse(drafts);
    }
}
