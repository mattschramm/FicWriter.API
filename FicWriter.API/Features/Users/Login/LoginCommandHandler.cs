using ErrorOr;
using FicWriter.API.Features.Users.Shared;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Security.Password;
using FicWriter.API.Infrastructure.Errors;
using MediatR;
using FicWriter.API.Infrastructure.Security.Tokens.Access;
using FicWriter.API.Infrastructure.Security.Tokens.Refresh;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data;

namespace FicWriter.API.Features.Users.Login;

public record LoginCommand(string Email, string Password) : IRequest<ErrorOr<UserResponse>>;

public class LoginCommandHandler(
    IUserReadOnly userReadOnly,
    IPasswordHasher passwordHasher,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    ITokenWriteOnly tokenWriteOnly,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, ErrorOr<UserResponse>>
{
    private readonly IUserReadOnly _userReadOnly = userReadOnly;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly ITokenWriteOnly _tokenWriteOnly = tokenWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<UserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userReadOnly.GetByEmail(request.Email);

        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
        {
            return UserErrors.InvalidCredentials();
        }

        var accessToken = _accessTokenGenerator.Generate(user.UserIdentifier);
        var refreshToken = _refreshTokenGenerator.Generate(user.Id);

        await _tokenWriteOnly.Add(refreshToken);
        
        await _unitOfWork.Commit();

        return user.ToResponse(accessToken, refreshToken.Token);
    }
}
