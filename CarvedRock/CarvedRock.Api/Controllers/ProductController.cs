using CarvedRock.Data.Entities;
using CarvedRock.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CarvedRock.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductLogic _productLogic;

    public ProductController(ILogger<ProductController> logger, IProductLogic productLogic)
    {
        _logger = logger;
        _productLogic = productLogic;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get(string category = "all")
    {
        var userName = User.Identity?.IsAuthenticated??false ? User.Identity.Name : "";
        using (_logger.BeginScope("ScopeUser: {ScopeUser}, ScopeCat: {ScopeCat}",
            userName, category))
        {
            _logger.LogInformation(CarvedRockEvents.GettingProducts,
                "Getting products in API for {category} and {user}", category, userName);
            return await _productLogic.GetProductsForCategoryAsync(category);
        }

        //return _productLogic.GetProductsForCategory(category);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        //var product = await _productLogic.GetProductByIdAsync(id);
        _logger.LogDebug("Getting single product in API for {id}", id);
        var product = _productLogic.GetProductById(id);
        if (product != null)
        {
            return Ok(product);
        }
        _logger.LogWarning("No product found for ID: {id}", id);
        return NotFound();
    }
}