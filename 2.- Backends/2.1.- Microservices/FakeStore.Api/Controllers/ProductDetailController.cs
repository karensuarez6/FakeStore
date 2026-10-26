
using FakeStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeStore.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductDetailController : ControllerBase
{
    private readonly ProductDetailService _productDetailService;

    public ProductDetailController(ProductDetailService productDetailService)
    {
        _productDetailService = productDetailService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productDetailService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
}
