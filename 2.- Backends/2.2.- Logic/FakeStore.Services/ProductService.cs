
using System.Net.Http.Json;
using DTOs;

namespace FakeStore.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FakeStore");
    }

    public async Task<List<ProductDto>?> GetProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>("products");
    }
}
