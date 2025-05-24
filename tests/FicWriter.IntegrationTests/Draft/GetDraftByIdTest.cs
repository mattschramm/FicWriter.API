using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using FicWriter.IntegrationTests.Draft.Util;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Draft;

public class GetDraftByIdTest : FicWriterFixture
{
    private readonly API.Models.Draft _draft;
    private readonly string _workId;
    private readonly Guid _userIdentifier;

    public GetDraftByIdTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _draft = app.FirstDraft;
        _workId = app.EncryptedWorkId;
        _userIdentifier = app.UserIdentifier;
    }

    [Fact]
    public async Task Success()
    {
        var url = DraftUrlFactory.GetDraftUrl(_workId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet($"{url}/{_draft.Id}", token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetInt64().ShouldBe(_draft.Id);
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(_draft.Title);
        responseData.RootElement.GetProperty("order").GetInt32().ShouldBe((int)_draft.Order);
    }

    [Fact]
    public async Task Fail_NotFound()
    {
        var url = DraftUrlFactory.GetDraftUrl(_workId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet($"{url}/{999999}", token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }
}
