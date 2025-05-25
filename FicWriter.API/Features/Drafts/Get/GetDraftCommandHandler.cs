using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Mediator.Requests;
using FicWriter.API.Shared.Draft;
using MediatR;

namespace FicWriter.API.Features.Drafts.Get;

public record GetDraftCommand(long WorkId, long DraftId) : IWorkRequest<ErrorOr<DraftResponse>>;

public class GetDraftCommandHandler(IDraftRepository draftRepository, DraftMapper mapper) : IRequestHandler<GetDraftCommand, ErrorOr<DraftResponse>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly DraftMapper _mapper = mapper;

    public async Task<ErrorOr<DraftResponse>> Handle(GetDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await _draftRepository.GetDraftById(request.WorkId, request.DraftId);

        if (draft is null)
        {
            return DraftErrors.DraftNotFound();
        }

        return _mapper.ToResponse(draft);
    }
}
