using FicWriter.API.Infrastructure.Data.Repositories.Tokens;
using Moq;

namespace CommonTestUtils.Repositories;

public static class TokenUpdateOnlyBuilder
{
    public static ITokenUpdateOnly Build() => new Mock<ITokenUpdateOnly>().Object;
}
