
using System.Net.Http.Json;
using DTOs;

namespace FakeStore.Services;

public class ProductDetailService
{
    private readonly HttpClient _httpClient;

    public ProductDetailService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FakeStore");
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProductDto>($"products/{id}");
    }
}
