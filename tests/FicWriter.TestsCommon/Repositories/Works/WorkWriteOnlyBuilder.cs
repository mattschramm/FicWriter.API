using FicWriter.API.Infrastructure.Data.Repositories.Works;
using Moq;

namespace CommonTestUtils.Repositories.Works;

public static class WorkWriteOnlyBuilder
{
    public static IWorkWriteOnly Build() => new Mock<IWorkWriteOnly>().Object;
}
