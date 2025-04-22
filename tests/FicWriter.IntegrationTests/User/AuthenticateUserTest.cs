using FicWriter.API.Features.Users.Auth;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FicWriter.IntegrationTests.User;

public class AuthenticateUserTest : FicWriterFixture
{
    private const string URL = "/user/authenticate";

    private readonly string _name;
    private readonly string _refreshToken;

    public AuthenticateUserTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _name = app.GetUserName();
        _refreshToken = app.GetRefreshToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new AuthenticateUserRequest(_refreshToken);

        var response = await DoPost(URL, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
    }

    [Fact]
    public async Task ShouldFail_RefreshTokenNotFound()
    {
        var request = new AuthenticateUserRequest("invalidtoken");

        var response = await DoPost(URL, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldHaveSingleItem().GetString().ShouldBe("Invalid refresh token.");
    }
}
