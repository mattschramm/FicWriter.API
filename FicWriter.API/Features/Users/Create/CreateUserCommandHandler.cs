using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.Password;
using FicWriter.API.Infrastructure.Security.Tokens.Access;
using FicWriter.API.Infrastructure.Security.Tokens.Refresh;
using FicWriter.API.Shared.User;
using MediatR;

namespace FicWriter.API.Features.Users.Create;

public sealed record CreateUserCommand(string Name, string Email, string Password) : IRequest<ErrorOr<UserResponse>>;

public class CreateUserCommandHandler(
    IUserReadOnly userReadOnly,
    IUserWriteOnly userWriteOnly,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    ITokenWriteOnly tokenWriteOnly) 
        : IRequestHandler<CreateUserCommand, ErrorOr<UserResponse>>
{
    private readonly IUserReadOnly _userReadOnly = userReadOnly;
    private readonly IUserWriteOnly _userWriteOnly = userWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly ITokenWriteOnly _tokenWriteOnly = tokenWriteOnly;

    public async Task<ErrorOr<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userReadOnly.ExistsWithEmail(request.Email))
        {
            return UserErrors.EmailAlreadyExists(request.Email);
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);

        var user = request.ToUser(hashedPassword);

        await _userWriteOnly.Add(user);

        await _unitOfWork.Commit();

        var accessToken = _accessTokenGenerator.Generate(user.UserIdentifier);
        
        var refreshToken = new Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _refreshTokenGenerator.Generate(),
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        await _tokenWriteOnly.Add(refreshToken);

        await _unitOfWork.Commit();

        return user.ToResponse(accessToken, refreshToken.Token);
    }
}
