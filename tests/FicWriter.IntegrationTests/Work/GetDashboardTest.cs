using CommonTestUtils.Tokens;
using FicWriter.IntegrationTests.Config;
using Shouldly;
using System.Text.Json;

namespace FicWriter.IntegrationTests.Work;

public class GetDashboardTest : FicWriterFixture
{
    private const string URL = "/works?Page=1&PageSize=20";

    private readonly string _workTitle;
    private readonly List<API.Models.Work> _works;
    private readonly Guid _userIdentifier;

    public GetDashboardTest(FicWriterWebApplicationFactory app) : base(app)
    {
        _workTitle = app.WorkTitle;
        _userIdentifier = app.UserIdentifier;
        _works = app.Works;
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(URL, token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);

        /*        {
                    "total": 0,
          "works": [
            {
                        "id": "string",
              "title": "string",
              "shortDescription": "string",
              "updatedAt": "2025-05-10T02:07:59.846Z",
              "genres": [
                "None"
              ],
              "tags": [
                {
                            "id": 0,
                  "content": "string"
                }
              ]
            }
          ]
        }*/

        var total = responseData.RootElement.GetProperty("total").GetInt32();
        var works = responseData.RootElement.GetProperty("works").EnumerateArray();
        
        total.ShouldBe(_works.Count);
        
        foreach (var work in works)
        {
            var id = work.GetProperty("id").GetString();
            var title = work.GetProperty("title").GetString();
            var genres = work.GetProperty("genres").EnumerateArray().Select(g => g.GetString()).ToList();
            var tags = work.GetProperty("tags").EnumerateArray().Select(t => t.GetString()).ToList();
            
            id.ShouldNotBeNullOrEmpty();

            _works.ShouldContain(w => w.Title == title);

            genres.ShouldNotBeEmpty();

            tags.ShouldNotBeEmpty();
        }
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoGet($"{URL}&Title=TitleNotFound", token);
       
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Success_WithTag()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var tag = _works.First().Tags.First().Content;
        
        var response = await DoGet($"{URL}&tag={tag}", token);
        
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        
        using var responseBody = await response.Content.ReadAsStreamAsync();
        using var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var works = responseData.RootElement.GetProperty("works").EnumerateArray();
        
        foreach (var work in works)
        {
            var tags = work.GetProperty("tags").EnumerateArray().Select(t => t.GetString()).ToList();
            tags.ShouldContain(tag);
        }
    }

    [Fact]
    public async Task Fail_InvalidGenre()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet($"{URL}&genre=InvalidGenre", token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
    }
}
