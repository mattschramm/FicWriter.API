using System.Net.Http.Headers;
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

    protected async Task<HttpResponseMessage> DoDelete(string url, string token = "")
    {
        AuthorizeRequest(token);

        return await _httpClient.DeleteAsync(url);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
