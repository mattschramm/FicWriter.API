using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Work;

public class ArchiveWorkTest : FicWriterFixture
{
    private const string URL = "/work";

    private readonly string _workId;
    private readonly Guid _userIdentifier;

    public ArchiveWorkTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _workId = app.EncryptedWorkId;
        _userIdentifier = app.UserIdentifier;
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPatch($"{URL}/{_workId}", new { }, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("isArchived").GetBoolean().ShouldBeTrue();
    }

    [Fact]
    public async Task Fail_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        
        var response = await DoPatch($"{URL}/1234567890", new { }, token);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
        
        using var responseBody = await response.Content.ReadAsStreamAsync();  
        using var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldHaveSingleItem().GetString().ShouldBe("Work not found");
    }
}
