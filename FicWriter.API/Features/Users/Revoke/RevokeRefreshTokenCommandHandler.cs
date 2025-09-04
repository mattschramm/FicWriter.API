using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using MediatR;

namespace FicWriter.API.Features.Users.Revoke;

public record RevokeRefreshTokenCommand(Guid UserId) : IRequest<ErrorOr<Success>>;

public class RevokeRefreshTokenCommandHandler(ITokenRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<RevokeRefreshTokenCommand, ErrorOr<Success>>
{
    private readonly ITokenRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await _repository.Delete(request.UserId);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
