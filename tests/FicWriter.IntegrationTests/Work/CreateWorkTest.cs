using CommonTestUtils.Requests;
using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Work;

public class CreateWorkTest : FicWriterFixture
{
    private const string URL = "/works";

    private readonly Guid _uniqueIdentifier;

    public CreateWorkTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _uniqueIdentifier = app.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = CreateWorkRequestBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(_uniqueIdentifier);

        var response = await DoPost(URL, request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldNotBeNullOrEmpty();

        response.Headers.Location.ShouldNotBeNull();
    }

    [Fact]
    public async Task Fail_TitleEmpty()
    {
        var request = CreateWorkRequestBuilder.Build() with { Title = string.Empty };
        var token = JwtTokenGeneratorBuilder.Build().Generate(_uniqueIdentifier);
        
        var response = await DoPost(URL, request, token);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors");

        errors.TryGetProperty("Title", out var titleErrors).ShouldBeTrue();
        titleErrors.EnumerateArray().ShouldHaveSingleItem().GetString().ShouldBe("Title is required.");
    }
}
