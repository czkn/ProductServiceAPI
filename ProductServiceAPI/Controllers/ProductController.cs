using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductServiceAPI.DTOs;

namespace ProductServiceAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{sku}")]
        public IActionResult GetProductBySKU(string sku)
        {
            var productDto = _productRepository.GetProductDtoBySKU(sku);

            return productDto == null ? NotFound() : Ok(productDto);
        }

        [HttpPost("save-products-prices-and-inventory")]
        public IActionResult SaveProductsPricesAndInventory()
        {
            try
            {
                _productRepository.SavePrices();

                _productRepository.SaveProducts();

                _productRepository.SaveInventory();

                return Ok("Products saved successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to save products. Error: {ex.Message}");
            }
        }
    }
}
