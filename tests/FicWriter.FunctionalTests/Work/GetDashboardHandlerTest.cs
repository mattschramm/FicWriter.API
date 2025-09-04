using CommonTestUtils.Models;
using CommonTestUtils.Repositories;
using CommonTestUtils.Services;
using FicWriter.API.Features.Dashboard;
using Shouldly;

namespace FicWriter.FunctionalTests.Work;

public class GetDashboardHandlerTest
{
    private static GetDashboardCommandHandler CreateHandler(API.Models.User user, List<API.Models.Work> works, GetDashboardCommand command)
    {
        var workRepository = new WorkRepositoryBuilder().GetDashboard(works, user, command).Build();
        var currentUser = CurrentUserBuilder.Build(user);
        var mapper = new GetDashboardMapper(SqidsEncoderBuilder.Build());

        return new GetDashboardCommandHandler(workRepository, currentUser, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build().user;

        var works = WorkBuilder.Build(user, 5);

        var command = new GetDashboardCommand(
            Page: 1,
            PageSize: 20,
            Title: null,
            Genres: [],
            Tags: [],
            Order: null
        );

        var handler = CreateHandler(user, works, command);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBeFalse();
        result.Value.Total.ShouldBe(5);
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var user = UserBuilder.Build().user;

        var works = WorkBuilder.Build(user, 5);

        var command = new GetDashboardCommand(
            Page: 1,
            PageSize: 20,
            Title: "Random Title No Content",
            Genres: [],
            Tags: [],
            Order: null
        );
        var handler = CreateHandler(user, works, command);
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.IsError.ShouldBeFalse();
        result.Value.Total.ShouldBe(0);
    }
}
