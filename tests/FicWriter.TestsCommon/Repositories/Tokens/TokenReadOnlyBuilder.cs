using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using FicWriter.API.Models;
using Moq;

namespace CommonTestUtils.Repositories.Tokens;

public class TokenReadOnlyBuilder
{
    private readonly Mock<ITokenReadOnly> _tokenReadOnlyMock;

    public TokenReadOnlyBuilder() => _tokenReadOnlyMock = new Mock<ITokenReadOnly>();

    public ITokenReadOnly Build() => _tokenReadOnlyMock.Object;

    public TokenReadOnlyBuilder Get(RefreshToken refreshToken)
    {
        _tokenReadOnlyMock
            .Setup(x => x.Get(refreshToken.Token))
            .ReturnsAsync(refreshToken);
        return this;
    }
}
