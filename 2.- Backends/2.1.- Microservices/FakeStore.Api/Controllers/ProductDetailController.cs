
using FakeStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FakeStore.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
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
