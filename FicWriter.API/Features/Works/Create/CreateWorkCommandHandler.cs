using ErrorOr;
using FicWriter.API.Enums;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Create;

public record CreateWorkCommand(string Title, string Description, List<Genres> Genres, List<string> Tags) : IRequest<ErrorOr<CreateWorkResponse>>;

public record CreateWorkResponse(string Id, string Title);

public class CreateWorkCommandHandler(
    IWorkRepository repository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    CreateWorkMapper mapper) : IRequestHandler<CreateWorkCommand, ErrorOr<CreateWorkResponse>>
{
    private readonly IWorkRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly CreateWorkMapper _mapper = mapper;

    public async Task<ErrorOr<CreateWorkResponse>> Handle(CreateWorkCommand command, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var work = _mapper.ToWork(command, user.Id);

        await _repository.Create(work);

        await _unitOfWork.Commit();

        return _mapper.ToResponse(work);
    }
}
