using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Archive;

public record ArchiveWorkCommand(long Id) : IRequest<ErrorOr<ArchiveWorkResponse>>;
public record ArchiveWorkResponse(bool IsArchived);

public class ArchiveWorkCommandHandler(IWorkRepository workRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<ArchiveWorkCommand, ErrorOr<ArchiveWorkResponse>>
{
    private readonly IWorkRepository _workRepository = workRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<ArchiveWorkResponse>> Handle(ArchiveWorkCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var work = await _workRepository.GetWorkByIdWithTracking(user, request.Id, true);

        if (work is null)
        {
            return WorkErrors.WorkNotFound();
        }

        work.IsArchived = !work.IsArchived;

        _workRepository.Update(work);

        await _unitOfWork.Commit();

        return new ArchiveWorkResponse(work.IsArchived);
    }
}
