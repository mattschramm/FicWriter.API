using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using Moq;

namespace CommonTestUtils.Repositories.Tokens;

public static class TokenWriteOnlyBuilder
{
    public static ITokenWriteOnly Build() => new Mock<ITokenWriteOnly>().Object;
}
