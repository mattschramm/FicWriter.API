using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Mediator.Requests;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Infrastructure.Mediator.Behaviors;

public class ValidateWorkIdBehavior<TRequest, TResponse>(IWorkRepository workRepository, ICurrentUser currentUser) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IWorkRepository _workRepository = workRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        if (request.WorkId <= 0)
        {
            return (dynamic)WorkErrors.InvalidWorkId();
        }

        var exists = await _workRepository.Exists(user, request.WorkId);

        if (!exists)
        {
            return (dynamic)WorkErrors.WorkNotFound();
        }

        return await next();
    }
}
