using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.Password;
using FicWriter.API.Infrastructure.Security.Tokens.Access;
using FicWriter.API.Infrastructure.Security.Tokens.Refresh;
using FicWriter.API.Models;
using FicWriter.API.Shared.User;
using MediatR;

namespace FicWriter.API.Features.Users.Login;

public record LoginCommand(string Email, string Password) : IRequest<ErrorOr<UserResponse>>;

public class LoginCommandHandler(
    IUserReadOnly userReadOnly,
    IPasswordHasher passwordHasher,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    ITokenWriteOnly tokenWriteOnly,
    IUnitOfWork unitOfWork,
    UserResponseMapper mapper) : IRequestHandler<LoginCommand, ErrorOr<UserResponse>>
{
    private readonly IUserReadOnly _userReadOnly = userReadOnly;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly ITokenWriteOnly _tokenWriteOnly = tokenWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserResponseMapper _mapper = mapper;

    public async Task<ErrorOr<UserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userReadOnly.GetByEmail(request.Email);

        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
        {
            return UserErrors.InvalidCredentials();
        }

        var accessToken = _accessTokenGenerator.Generate(user.UserIdentifier);
        
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _refreshTokenGenerator.Generate(),
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        await _tokenWriteOnly.Add(refreshToken);
        
        await _unitOfWork.Commit();

        return _mapper.ToResponse(user, accessToken, refreshToken.Token);
    }
}
