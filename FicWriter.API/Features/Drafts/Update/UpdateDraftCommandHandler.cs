using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Mediator.Requests;
using MediatR;

namespace FicWriter.API.Features.Drafts.Update;

public record UpdateDraftCommand(long WorkId, long DraftId, string Title) : IWorkRequest<ErrorOr<Success>>;

public class UpdateDraftCommandHandler(IDraftRepository draftRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateDraftCommand, ErrorOr<Success>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(UpdateDraftCommand request, CancellationToken cancellationToken)
    {
        var draft = await _draftRepository.GetDraftByIdWithTracking(request.WorkId, request.DraftId);

        if (draft is null)
        {
            return DraftErrors.DraftNotFound();
        }

        draft.Title = request.Title;
        draft.UpdatedAt = DateTime.UtcNow;

        _draftRepository.Update(draft);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}