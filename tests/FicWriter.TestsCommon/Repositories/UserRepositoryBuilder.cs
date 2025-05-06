using FicWriter.API.Infrastructure.Data.Repositories.Users;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories;

public class UserRepositoryBuilder
{
    private readonly Mock<IUserRepository> _repositoryMock;

    public UserRepositoryBuilder()
    {
        _repositoryMock = new Mock<IUserRepository>();
    }

    public UserRepositoryBuilder ExistsWithEmail(string email)
    {
        _repositoryMock.Setup(x => x.ExistsWithEmail(email))
            .ReturnsAsync(true);
        return this;
    }

    public UserRepositoryBuilder GetByEmail(User user)
    {
        _repositoryMock.Setup(x => x.GetByEmail(user.Email))
            .ReturnsAsync(user);
        return this;
    }

    public UserRepositoryBuilder GetByIdWithTracking(User user)
    {
        _repositoryMock.Setup(x => x.GetByIdWithTracking(user.Id))
            .ReturnsAsync(user);
        return this;
    }

    public IUserRepository Build() => _repositoryMock.Object;
}
