using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Drafts.Delete;

public record DeleteDraftCommand(long DraftId, long WorkId) : IRequest<ErrorOr<Success>>;

public class DeleteDraftCommandHandler(IDraftRepository draftRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDraftCommand, ErrorOr<Success>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteDraftCommand request, CancellationToken cancellationToken)
    {
        var success = await _draftRepository.Delete(request.DraftId);

        if (!success)
        {
            return DraftErrors.DraftNotFound();
        }

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
