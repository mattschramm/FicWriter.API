using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using Moq;

namespace CommonTestUtils.Repositories;

public static class TokenWriteOnlyBuilder
{
    public static ITokenWriteOnly Build() => new Mock<ITokenWriteOnly>().Object;
}
