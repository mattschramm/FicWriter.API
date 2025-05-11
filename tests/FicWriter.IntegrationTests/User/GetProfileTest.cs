using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.User;

public class GetProfileTest : FicWriterFixture
{
    private const string URL = "/user";

    private readonly string _name;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public GetProfileTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _name = app.UserName;
        _email = app.UserEmail;
        _userIdentifier = app.UserIdentifier;
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(URL, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        var responseName = responseData.RootElement.GetProperty("name").GetString();
        var responseEmail = responseData.RootElement.GetProperty("email").GetString();

        responseName.ShouldBe(_name);
        responseEmail.ShouldBe(_email);
    }

    [Fact]
    public async Task Unauthorized()
    {
        var response = await DoGet(URL);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
    }
}
