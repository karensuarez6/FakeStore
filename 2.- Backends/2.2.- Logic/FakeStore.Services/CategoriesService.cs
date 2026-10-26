
using System.Net.Http.Json;

namespace FakeStore.Services;

public class CategoriesService
{
    private readonly HttpClient _httpClient;

    public CategoriesService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FakeStore");
    }

    public async Task<List<string>?> GetCategoriesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<string>>("products/categories");
    }
}
