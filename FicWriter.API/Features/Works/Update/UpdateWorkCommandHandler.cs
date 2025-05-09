using ErrorOr;
using FicWriter.API.Enums;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Update;

public record UpdateWorkCommand(long Id, string Title, string Description, List<Genres> Genres, List<string> Tags) : IRequest<ErrorOr<Success>>;

public class UpdateWorkCommandHandler(IWorkRepository repository, ICurrentUser currentUser, IUnitOfWork unitOfWork, UpdateWorkMapper mapper) : IRequestHandler<UpdateWorkCommand, ErrorOr<Success>>
{
    private readonly IWorkRepository _repository = repository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UpdateWorkMapper _mapper = mapper;

    public async Task<ErrorOr<Success>> Handle(UpdateWorkCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var work = await _repository.GetWorkByIdWithTracking(user, request.Id);

        if (work is null)
        {
            return WorkErrors.WorkNotFound();
        }

        _mapper.ToUpdatedWork(work, request);

        _repository.Update(work);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
