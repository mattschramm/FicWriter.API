using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Create;

public record CreateWorkCommand(string Title, string Description) : IRequest<ErrorOr<CreateWorkResponse>>;

public record CreateWorkResponse(string Id, string Title);

public class CreateWorkCommandHandler(
    IWorkWriteOnly workWriteOnly,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    CreateWorkMapper mapper) : IRequestHandler<CreateWorkCommand, ErrorOr<CreateWorkResponse>>
{
    private readonly IWorkWriteOnly _workWriteOnly = workWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly CreateWorkMapper _mapper = mapper;

    public async Task<ErrorOr<CreateWorkResponse>> Handle(CreateWorkCommand command, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var work = _mapper.ToWork(command, user.Id);

        await _workWriteOnly.Create(work);

        await _unitOfWork.Commit();

        return _mapper.ToResponse(work);
    }
}
