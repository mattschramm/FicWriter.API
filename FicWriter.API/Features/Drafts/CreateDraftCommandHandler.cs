using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Drafts;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Drafts;

public record CreateDraftCommand(string Title, uint Order, long WorkId) : IRequest<ErrorOr<CreateDraftResponse>>;
public record CreateDraftResponse(long Id, string Title, DateTime CreatedAt, DateTime UpdatedAt, uint Order);

public class CreateDraftCommandHandler(IDraftRepository draftRepository, IUnitOfWork unitOfWork, IWorkRepository workRepository, ICurrentUser currentUser, CreateDraftMapper mapper) : IRequestHandler<CreateDraftCommand, ErrorOr<CreateDraftResponse>>
{
    private readonly IDraftRepository _draftRepository = draftRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IWorkRepository _workRepository = workRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly CreateDraftMapper _mapper = mapper;

    public async Task<ErrorOr<CreateDraftResponse>> Handle(CreateDraftCommand command, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        if (!(await _workRepository.Exists(user, command.WorkId)))
        {
            return WorkErrors.WorkNotFound();
        }

        var draft = _mapper.ToDraft(command);

        await _draftRepository.Create(draft);

        await _unitOfWork.Commit();

        return _mapper.ToResponse(draft);
    }
}
