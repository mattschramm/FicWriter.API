using ErrorOr;
using FicWriter.API.Infrastructure.Data;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Users.Update;

public record UpdateUserCommand(string Name, string Email) : IRequest<ErrorOr<Success>>;

public class UpdateUserCommandHandler(IUserUpdateOnly userUpdateOnly, IUnitOfWork unitOfWork, ICurrentUser currentUser, IUserReadOnly userReadOnly) : IRequestHandler<UpdateUserCommand, ErrorOr<Success>>
{
    private readonly IUserUpdateOnly _userUpdateOnly = userUpdateOnly;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserReadOnly _userReadOnly = userReadOnly;

    public async Task<ErrorOr<Success>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        if (!currentUser.Email.Equals(request.Email))
        {
            var emailExists = await _userReadOnly.ExistsWithEmail(request.Email);

            if (emailExists)
                return UserErrors.EmailAlreadyExists();
        }

        var userToUpdate = await _userUpdateOnly.GetUserByIdWithTracking(currentUser.Id);

        userToUpdate!.Name = request.Name;
        userToUpdate.Email = request.Email;

        _userUpdateOnly.Update(userToUpdate);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
