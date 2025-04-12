using FicWriter.API.Infrastructure.Data.Repositories.Users;
using Moq;

namespace CommonTestUtils.Repositories;

public static class UserWriteOnlyBuilder
{
    public static IUserWriteOnly Build() => new Mock<IUserWriteOnly>().Object;
}
