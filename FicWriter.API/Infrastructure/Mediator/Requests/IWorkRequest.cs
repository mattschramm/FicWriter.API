using ErrorOr;
using MediatR;

namespace FicWriter.API.Infrastructure.Mediator.Requests;

public interface IWorkRequest<TResponse> : IRequest<TResponse>
{
    long WorkId { get; }
}
