using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using MediatR;

namespace FicWriter.API.Features.Users.Revoke;

public record RevokeRefreshTokenCommand(Guid UserId) : IRequest<ErrorOr<Success>>;

public class RevokeRefreshTokenCommandHandler(ITokenWriteOnly tokenWriteOnly, IUnitOfWork unitOfWork) : IRequestHandler<RevokeRefreshTokenCommand, ErrorOr<Success>>
{
    private readonly ITokenWriteOnly _tokenWriteOnly = tokenWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Success>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await _tokenWriteOnly.Delete(request.UserId);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
