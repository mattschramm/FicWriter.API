using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Encoding.Password;
using FicWriter.API.Infrastructure.Errors;
using MediatR;

namespace FicWriter.API.Features.Users.Create;

public class CreateUserCommandHandler(IUserReadOnly userReadOnly, IUserWriteOnly userWriteOnly, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, ErrorOr<CreateUserResponse>>
{
    private readonly IUserReadOnly _userReadOnly = userReadOnly;
    private readonly IUserWriteOnly _userWriteOnly = userWriteOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<ErrorOr<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userReadOnly.ExistsWithEmail(request.Email))
        {
            return UserErrors.EmailAlreadyExists(request.Email);
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);

        var user = request.ToUser(hashedPassword);

        await _userWriteOnly.Create(user);
        await _unitOfWork.Commit();

        return user.ToResponse();
    }
}
