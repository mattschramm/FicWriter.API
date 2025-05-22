using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Mediator.Requests;
using MediatR;
using Sqids;

namespace FicWriter.API.Features.Drafts.Get;

public record GetDraftCommand(long WorkId, long DraftId) : IWorkRequest<ErrorOr<GetDraftResponse>>;

public record GetDraftResponse(long Id, string Title, DateTime CreatedAt, DateTime UpdatedAt, uint Order);

public class GetDraftCommandHandler(IDraftRepository draftRepository, GetDraftMapper mapper) : IRequestHandler<GetDraftCommand, ErrorOr<GetDraftResponse>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly GetDraftMapper _mapper = mapper;

    public async Task<ErrorOr<GetDraftResponse>> Handle(GetDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await _draftRepository.GetDraftById(request.WorkId, request.DraftId);

        if (draft is null)
        {
            return DraftErrors.DraftNotFound();
        }

        return _mapper.ToResponse(draft);
    }
}
