using FicWriter.API.Infrastructure.Data.Repositories.Works;
using Moq;

namespace CommonTestUtils.Repositories;

public static class WorkWriteOnlyBuilder
{
    public static IWorkWriteOnly Build() => new Mock<IWorkWriteOnly>().Object;
}
