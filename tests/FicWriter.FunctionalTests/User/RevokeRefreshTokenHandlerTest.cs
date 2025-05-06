using CommonTestUtils.Repositories;
using FicWriter.API.Features.Users.Revoke;
using Shouldly;

namespace FicWriter.FunctionalTests.User;

public class RevokeRefreshTokenHandlerTest
{
    private static RevokeRefreshTokenCommandHandler CreateHandler()
    {
        var tokenRepository = new TokenRepositoryBuilder().Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new RevokeRefreshTokenCommandHandler(tokenRepository, unitOfWork);
    }

    [Fact]
    public async Task Success()
    {
        var handler = CreateHandler();
        var userId = Guid.NewGuid();
        var command = new RevokeRefreshTokenCommand(userId);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
    }
}
