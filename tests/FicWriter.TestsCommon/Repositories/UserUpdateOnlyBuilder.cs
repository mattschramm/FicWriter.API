using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories;

public class UserUpdateOnlyBuilder
{
    private readonly Mock<IUserUpdateOnly> _userUpdateOnly;

    public UserUpdateOnlyBuilder()
    {
        _userUpdateOnly = new Mock<IUserUpdateOnly>();
    }

    public UserUpdateOnlyBuilder GetUserByIdWithTracking(User user)
    {
        _userUpdateOnly.Setup(x => x.GetUserByIdWithTracking(user.Id))
            .ReturnsAsync(user);
        return this;
    }

    public IUserUpdateOnly Build() => _userUpdateOnly.Object;
}
