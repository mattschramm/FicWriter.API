using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Delete;

public record DeleteWorkCommand(long Id) : IRequest<ErrorOr<Success>>;

public class DeleteWorkCommandHandler(IWorkWriteOnly workWriteOnly, IWorkReadOnly workReadOnly, ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkCommand, ErrorOr<Success>>
{
    private readonly IWorkWriteOnly _workWriteOnly = workWriteOnly;
    private readonly IWorkReadOnly _workReadOnly = workReadOnly;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(DeleteWorkCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        if (!await _workReadOnly.Exists(user, request.Id))
        {
            return WorkErrors.WorkNotFound();
        }

        await _workWriteOnly.Delete(request.Id);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
