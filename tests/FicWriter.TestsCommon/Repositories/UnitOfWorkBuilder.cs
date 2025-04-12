using FicWriter.API.Infrastructure.Data;
using Moq;

namespace CommonTestUtils.Repositories;

public static class UnitOfWorkBuilder
{
    public static IUnitOfWork Build() => new Mock<IUnitOfWork>().Object;
}
