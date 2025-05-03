using CommonTestUtils.Services;
using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Work;

public class GetWorkByIdTest : FicWriterFixture
{
    private const string URL = "/works";

    private readonly string _workId;
    private readonly string _workTitle;
    private readonly string _workDescription;
    private readonly Guid _userIdentifier;

    public GetWorkByIdTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _workId = app.GetEncryptedWorkId();
        _workTitle = app.GetWorkTitle();
        _workDescription = app.GetWorkDescription();
        _userIdentifier = app.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet($"{URL}/{_workId}", token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldBe(_workId);
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(_workTitle);
        responseData.RootElement.GetProperty("description").GetString().ShouldBe(_workDescription);
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var fakeId = SqidsEncoderBuilder.Build().Encode(0);

        var response = await DoGet($"{URL}/{fakeId}", token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.ShouldHaveSingleItem().GetString().ShouldBe("Work not found");
    }
}
