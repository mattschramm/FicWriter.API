using CommonTestUtils.Requests;
using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.User;

public class UpdateUserTest : FicWriterFixture
{
    private const string URL = "user";

    private readonly Guid _userIdentifier;

    public UpdateUserTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _userIdentifier = app.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = UpdateUserCommandBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(URL, request, token);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        var user = await DoGet(URL, token);

        using var responseBody = await user.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var responseName = responseData.RootElement.GetProperty("name").GetString();
        var responseEmail = responseData.RootElement.GetProperty("email").GetString();

        responseName.ShouldBe(request.Name);
        responseEmail.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Fail_EmptyName()
    {
        var request = UpdateUserCommandBuilder.Build() with { Name = "" };
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        
        var response = await DoPut(URL, request, token);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        
        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors");
        errors.TryGetProperty("Name", out var nameErrors).ShouldBeTrue();
        nameErrors.EnumerateArray().Select(x => x.GetString()).ShouldContain("Name is required.");
    }

    [Fact]
    public async Task Fail_InvalidEmail()
    {
        var request = UpdateUserCommandBuilder.Build() with { Email = "invalidemail" };
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(URL, request, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors");
        errors.TryGetProperty("Email", out var emailErrors).ShouldBeTrue();
        emailErrors.EnumerateArray().Select(x => x.GetString()).ShouldContain("Email is not valid.");
    }
}
