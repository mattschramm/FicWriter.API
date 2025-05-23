using CommonTestUtils.Tokens;
using FicWriter.API.Features.Drafts.Create;
using FicWriter.IntegrationTests.Config;
using FicWriter.IntegrationTests.Draft.Util;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Draft;

public class CreateDraftTest : FicWriterFixture
{
    private readonly Guid _userIdentifier;
    private readonly string _workId;

    public CreateDraftTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _userIdentifier = app.UserIdentifier;
        _workId = app.EncryptedWorkId;
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder
            .Build()
            .Generate(_userIdentifier);

        var request = new CreateDraftRequest("Test Title", 1);

        var url = DraftUrlFactory.GetDraftUrl(_workId);

        var response = await DoPost(url, request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
        responseData.RootElement.GetProperty("order").GetUInt32().ShouldBe(request.Order);
    }

    [Fact]
    public async Task Fail_TitleEmpty()
    {
        var token = JwtTokenGeneratorBuilder
            .Build()
            .Generate(_userIdentifier);

        var request = new CreateDraftRequest(string.Empty, 1);

        var url = DraftUrlFactory.GetDraftUrl(_workId);

        var response = await DoPost(url, request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors");

        errors.TryGetProperty("Title", out var titleErrors).ShouldBeTrue();
        titleErrors.EnumerateArray().ShouldHaveSingleItem().GetString().ShouldBe("Title is required");
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var token = JwtTokenGeneratorBuilder
            .Build()
            .Generate(_userIdentifier);

        var request = new CreateDraftRequest("Test Title", 1);

        var url = DraftUrlFactory.GetDraftUrl("123456789");

        var response = await DoPost(url, request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().ShouldBe("Work not found");
    }
}
