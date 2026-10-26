
using FakeStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeStore.Api.Controllers;

[ApiController]
[Route("api/products/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoriesService.GetCategoriesAsync();
        return Ok(categories);
    }
}
