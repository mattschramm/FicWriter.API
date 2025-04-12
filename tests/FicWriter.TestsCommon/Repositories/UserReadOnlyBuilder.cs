using FicWriter.API.Infrastructure.Data.Repositories.Users;
using Moq;

namespace CommonTestUtils.Repositories;

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
}
