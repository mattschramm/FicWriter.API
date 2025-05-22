using ErrorOr;
using FicWriter.API.Features.Users.Common;
using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Security.Password;
using FicWriter.API.Infrastructure.Security.Tokens.Access;
using FicWriter.API.Infrastructure.Security.Tokens.Refresh;
using MediatR;

namespace FicWriter.API.Features.Users.Create;

public sealed record CreateUserCommand(string Name, string Email, string Password) : IRequest<ErrorOr<UserResponse>>;

public class CreateUserCommandHandler(
    ITokenRepository tokenRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    CreateUserMapper mapper) 
        : IRequestHandler<CreateUserCommand, ErrorOr<UserResponse>>
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
    private readonly CreateUserMapper _mapper = mapper;

    public async Task<ErrorOr<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsWithEmail(request.Email))
        {
            return UserErrors.EmailAlreadyExists();
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);

        var user = _mapper.ToUser(request, hashedPassword);
        user.UserIdentifier = Guid.NewGuid();

        await _userRepository.Add(user);

        await _unitOfWork.Commit();

        var accessToken = _accessTokenGenerator.Generate(user.UserIdentifier);
        
        var refreshToken = new Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _refreshTokenGenerator.Generate(),
            UserId = user.Id,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        await _tokenRepository.Add(refreshToken);

        await _unitOfWork.Commit();

        return _mapper.ToResponse(user, accessToken, refreshToken.Token);
    }
}
