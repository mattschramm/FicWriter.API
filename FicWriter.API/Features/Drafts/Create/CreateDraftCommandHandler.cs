using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Mediator.Requests;
using MediatR;

namespace FicWriter.API.Features.Drafts.Create;

public record CreateDraftCommand(string Title, long WorkId) : IWorkRequest<ErrorOr<CreateDraftResponse>>;

public record CreateDraftResponse(long Id, string Title, DateTime CreatedAt, DateTime UpdatedAt, uint Order);

public class CreateDraftCommandHandler(IDraftRepository draftRepository, IUnitOfWork unitOfWork, CreateDraftMapper mapper) : IRequestHandler<CreateDraftCommand, ErrorOr<CreateDraftResponse>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly CreateDraftMapper _mapper = mapper;

    public async Task<ErrorOr<CreateDraftResponse>> Handle(CreateDraftCommand command, CancellationToken cancellationToken)
    {
        var draft = _mapper.ToDraft(command);

        draft.Order = await _draftRepository.GetNextOrder(command.WorkId);

        await _draftRepository.Create(draft);

        await _unitOfWork.Commit();

        return _mapper.ToResponse(draft);
    }
}
