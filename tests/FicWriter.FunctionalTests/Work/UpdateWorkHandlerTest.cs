using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Repositories.Works;
using CommonTestUtils.Requests;
using CommonTestUtils.Services;
using FicWriter.API.Features.Works.Update;
using FicWriter.API.Models;
using Shouldly;

namespace FicWriter.FunctionalTests.Work;

public class UpdateWorkHandlerTest
{
    private static UpdateWorkCommandHandler CreateHandler(API.Models.User user, API.Models.Work? work = null)
    {
        var workUpdateOnlyBuilder = new WorkUpdateOnlyBuilder();

        if (work is not null)
        {
            workUpdateOnlyBuilder.GetWorkByIdWithTracking(user, work);
        }

        var workUpdateOnly = workUpdateOnlyBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new UpdateWorkCommandHandler(workUpdateOnly, currentUser, unitOfWork);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;
        var work = WorkBuilder.Build(user);
        var request = UpdateWorkCommandBuilder.Build(work.Id);
        var handler = CreateHandler(user, work);

        var result = await handler.Handle(request, CancellationToken.None);

        result.IsError.ShouldBeFalse();
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var user = UserBuilder.Build().user;
        var work = WorkBuilder.Build(user);
        var request = UpdateWorkCommandBuilder.Build(work.Id);
        
        var handler = CreateHandler(user);
        
        var result = await handler.Handle(request, CancellationToken.None);
        
        result.IsError.ShouldBeTrue();
        result.FirstError.Code.ShouldBe("Work.NotFound");
        result.FirstError.Description.ShouldBe("Work not found");
    }
}
