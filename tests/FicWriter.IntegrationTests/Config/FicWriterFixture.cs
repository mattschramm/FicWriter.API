using System.Net.Http.Json;

namespace FicWriter.IntegrationTests.Config;

public class FicWriterFixture : IClassFixture<FicWriterWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public FicWriterFixture(FicWriterWebApplicationFactory app)
    {
        _httpClient = app.CreateClient();
    }

    protected async Task<HttpResponseMessage> DoPost(string url, object request)
    {
        return await _httpClient.PostAsJsonAsync(url, request);
    }
}
