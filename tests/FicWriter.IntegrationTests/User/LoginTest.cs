using CommonTestUtils.Requests;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.User;

public class LoginTest : FicWriterFixture
{
    private const string URL = "/user/login";

    private readonly string _name;
    private readonly string _password;
    private readonly string _email;

    public LoginTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _name = app.GetUserName();
        _password = app.GetUserPassword();
        _email = app.GetUserEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = LoginRequestBuilder.Build(_email, _password);

        var response = await DoPost(URL, request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseData.RootElement.GetProperty("tokens").EnumerateObject().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task ShouldFail_UserNotFound()
    {
        var request = LoginRequestBuilder.Build("invalid@email.com", "invalidpassword");

        var response = await DoPost(URL, request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldHaveSingleItem().GetString().ShouldBe("Invalid email or password.");
    }

    [Fact]
    public async Task ShouldFail_WithEmptyPassword()
    {
        var request = LoginRequestBuilder.Build(_email, "");
        var response = await DoPost(URL, request);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        
        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors");

        errors.TryGetProperty("Password", out var passwordErrors).ShouldBeTrue();
        passwordErrors.EnumerateArray().Select(x => x.GetString()).ShouldContain("Password is required.");
    }
}
