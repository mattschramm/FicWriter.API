using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using FicWriter.IntegrationTests.Draft.Util;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Draft;

public class ListDraftsTest : FicWriterFixture
{
    private readonly List<API.Models.Draft> _drafts;
    private readonly string _workId;
    private readonly Guid _userIdentifier;

    public ListDraftsTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _drafts = app.Drafts;
        _workId = app.EncryptedWorkId;
        _userIdentifier = app.UserIdentifier;
    }

    [Fact]
    public async Task Success()
    {
        var url = DraftUrlFactory.GetDraftUrl(_workId);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoGet(url, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var drafts = responseData.RootElement.GetProperty("drafts").EnumerateArray();
        drafts.Count().ShouldBe(_drafts.Count);
    }
}
