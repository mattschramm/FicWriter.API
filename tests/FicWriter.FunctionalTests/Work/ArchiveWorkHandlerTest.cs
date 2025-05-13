using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Services;
using FicWriter.API.Features.Works.Archive;
using Shouldly;

namespace FicWriter.FunctionalTests.Work;

public class ArchiveWorkHandlerTest
{
    private static ArchiveWorkCommandHandler CreateHandler(API.Models.User user, API.Models.Work? work = null)
    {
        var workRepositoryBuilder = new WorkRepositoryBuilder();

        if (work is not null)
        {
            workRepositoryBuilder.GetWorkByIdWithTracking(work, user, true);
        }

        var workRepository = workRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        return new ArchiveWorkCommandHandler(
            workRepository,
            currentUser,
            unitOfWork);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        
        var work = WorkBuilder.Build(user);
        
        var handler = CreateHandler(user, work);
        
        var command = new ArchiveWorkCommand(work.Id);
        
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.IsArchived.ShouldBeTrue();
    }

    [Fact]
    public async Task Success_Disarchive()
    {
        var user = UserBuilder.Build().user;

        var work = WorkBuilder.Build(user);
        work.IsArchived = true;

        var handler = CreateHandler(user, work);

        var command = new ArchiveWorkCommand(work.Id);

        var result = await handler.Handle(command, CancellationToken.None);
        result.IsError.ShouldBeFalse();
        result.Value.IsArchived.ShouldBeFalse();
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var user = UserBuilder.Build().user;

        var work = WorkBuilder.Build(user);

        var handler = CreateHandler(user);

        var command = new ArchiveWorkCommand(work.Id);

        var result = await handler.Handle(command, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
    }
}
