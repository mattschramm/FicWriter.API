using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Delete;

public record DeleteWorkCommand(long Id) : IRequest<ErrorOr<Success>>;

public class DeleteWorkCommandHandler(IWorkRepository repository, ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkCommand, ErrorOr<Success>>
{
    private readonly IWorkRepository _repository = repository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteWorkCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        if (!await _repository.Exists(user, request.Id))
        {
            return WorkErrors.WorkNotFound();
        }

        await _repository.Delete(request.Id);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
