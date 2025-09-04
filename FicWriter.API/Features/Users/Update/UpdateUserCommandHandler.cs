using ErrorOr;
using FicWriter.API.Infrastructure.Data.Repositories.Unit;
using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Users.Update;

public record UpdateUserCommand(string Name, string Email) : IRequest<ErrorOr<Success>>;

public class UpdateUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateUserCommand, ErrorOr<Success>>
{
    private readonly IUserRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<ErrorOr<Success>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        if (!currentUser.Email.Equals(request.Email))
        {
            var emailExists = await _repository.ExistsWithEmail(request.Email);

            if (emailExists)
                return UserErrors.EmailAlreadyExists();
        }

        var userToUpdate = await _repository.GetByIdWithTracking(currentUser.Id);

        userToUpdate!.Name = request.Name;
        userToUpdate.Email = request.Email;

        _repository.Update(userToUpdate);

        await _unitOfWork.Commit();

        return Result.Success;
    }
}
