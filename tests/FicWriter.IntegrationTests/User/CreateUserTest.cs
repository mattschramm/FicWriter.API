using CommonTestUtils.Requests;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.User;

public class CreateUserTest : FicWriterFixture
{
    private const string URL = "/user";

    public CreateUserTest(FicWriterWebApplicationFactory app) : base(app)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = CreateUserRequestBuilder.Build();

        var response = await DoPost(URL, request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("accessToken").EnumerateObject().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task ShouldFaild_WithEmptyName()
    {
        var request = CreateUserRequestBuilder.Build() with { Name = "" };

        var response = await DoPost(URL, request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors");

        errors.TryGetProperty("Name", out var nameErrors).ShouldBeTrue();

        nameErrors.EnumerateArray().Select(x => x.GetString()).ShouldContain("Name is required.");
    }
}
