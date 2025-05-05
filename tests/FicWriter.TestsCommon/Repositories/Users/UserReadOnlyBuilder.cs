using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories.Users;

public class UserReadOnlyBuilder
{
    private readonly Mock<IUserReadOnly> _userReadOnlyMock;

    public UserReadOnlyBuilder() => _userReadOnlyMock = new Mock<IUserReadOnly>();

    public IUserReadOnly Build() => _userReadOnlyMock.Object;

    public UserReadOnlyBuilder ExistsWithEmail(string email)
    {
        _userReadOnlyMock
            .Setup(x => x.ExistsWithEmail(email))
            .ReturnsAsync(true);
        return this;
    }

    public UserReadOnlyBuilder GetByEmail(User user)
    {
        _userReadOnlyMock
            .Setup(x => x.GetByEmail(user.Email))
            .ReturnsAsync(user);
        return this;
    }
}
