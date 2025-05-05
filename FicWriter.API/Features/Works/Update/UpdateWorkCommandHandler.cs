using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Works.Update;

public record UpdateWorkCommand(long Id, string Title, string Description) : IRequest<ErrorOr<Success>>;

public class UpdateWorkCommandHandler(IWorkUpdateOnly workUpdateOnly, ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<UpdateWorkCommand, ErrorOr<Success>>
{
    private readonly IWorkUpdateOnly _workUpdateOnly = workUpdateOnly;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(UpdateWorkCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var work = await _workUpdateOnly.GetWorkByIdWithTracking(user, request.Id);

        if (work is null)
        {
            return Error.NotFound(description: $"Work not found.");
        }

        work.Title = request.Title;
        work.Description = request.Description;
        work.UpdatedAt = DateTime.UtcNow;

        _workUpdateOnly.Update(work);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
