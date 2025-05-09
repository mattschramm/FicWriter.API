using CommonTestUtils.Tokens;
using FicWriter.API.Features.Works.Update;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Work;

public class UpdateWorkTest : FicWriterFixture
{
    private const string URL = "/work";

    private readonly string _workId;
    private readonly Guid _userIdentifier;
    
    public UpdateWorkTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _Title = app.GetWorkTitle();
        _Description = app.GetWorkDescription();
        _workId = app.GetEncryptedWorkId();
        _userIdentifier = app.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new UpdateWorkRequest("New Title", "New Description");

        var response = await DoPut($"{URL}/{_workId}", request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        var work = await DoGet($"{URL}/{_workId}", token);

        using var responseBody = await work.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);
        var responseTitle = responseData.RootElement.GetProperty("title").GetString();
        var responseDescription = responseData.RootElement.GetProperty("description").GetString();

        responseTitle.ShouldBe(request.Title);
        responseDescription.ShouldBe(request.Description);
    }

    [Fact]
    public async Task Fail_WorkNotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = new UpdateWorkRequest("New Title", "New Description");
        
        var response = await DoPut($"{URL}/invalidId", request, token);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
        
        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        errors.ShouldHaveSingleItem().GetString().ShouldBe("Work not found");
    }
}
