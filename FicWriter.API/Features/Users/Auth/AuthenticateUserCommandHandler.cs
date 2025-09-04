using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.Tokens.Access;
using FicWriter.API.Infrastructure.Security.Tokens.Refresh;
using FicWriter.API.Shared.User;
using MediatR;

namespace FicWriter.API.Features.Users.Auth;

public record AuthenticateUserCommand(string RefreshToken) : IRequest<ErrorOr<UserResponse>>;

public class AuthenticateUserCommandHandler(
    ITokenRepository repository,
    IUnitOfWork unitOfWork,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    UserResponseMapper mapper) : IRequestHandler<AuthenticateUserCommand, ErrorOr<UserResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly UserResponseMapper _mapper = mapper;
    private readonly ITokenRepository _repository = repository;

    public async Task<ErrorOr<UserResponse>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _repository.Get(request.RefreshToken);

        if (refreshToken is null || refreshToken.IsExpired())
        {
            return TokenErrors.InvalidRefreshToken();
        }

        var accessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier);

        refreshToken.Token = _refreshTokenGenerator.Generate();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        _repository.Update(refreshToken);

        await _unitOfWork.Commit();

        return _mapper.ToResponse(refreshToken.User, accessToken, refreshToken.Token);
    }
}
