using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using MediatR;
using System.Text;

namespace FicWriter.API.Features.Works.Create;

public record CreateWorkCommand(string Title, string Description) : IRequest<ErrorOr<CreateWorkResponse>>;

public record CreateWorkResponse(long Id, string Title);

public class CreateWorkCommandHandler(IWorkWriteOnly workWriteOnly, IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<CreateWorkCommand, ErrorOr<CreateWorkResponse>>
{
    private readonly IWorkWriteOnly _workWriteOnly = workWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<ErrorOr<CreateWorkResponse>> Handle(CreateWorkCommand command, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var work = command.ToWork(user.Id);

        await _workWriteOnly.Create(work);

        await _unitOfWork.Commit();

        return work.ToResponse();
    }
}
