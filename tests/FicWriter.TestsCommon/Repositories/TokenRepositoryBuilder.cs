using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories;

public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _repositoryMock;

    public TokenRepositoryBuilder()
    {
        _repositoryMock = new Mock<ITokenRepository>();
    }

    public TokenRepositoryBuilder Get(RefreshToken refreshToken)
    {
        _repositoryMock
            .Setup(x => x.Get(refreshToken.Token))
            .ReturnsAsync(refreshToken);
        return this;
    }

    public ITokenRepository Build() => _repositoryMock.Object;
}
