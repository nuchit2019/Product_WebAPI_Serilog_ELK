using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductWebAPISerilog.Models;
using ProductWebAPISerilog.Repository;
using Serilog;

namespace ProductWebAPISerilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            Log.Information("Retrieved {Count} products", products.Count());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                Log.Warning("Product with ID {Id} not found", id);
                return NotFound();
            }

            Log.Information("Retrieved product with ID {Id}", id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await _productRepository.AddProductAsync(product);
            Log.Information("Added new product with ID {Id}", product.Id);
            Log.Error("TEST ERROR: Added new product with ID {Id}", product.Id);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                Log.Warning("ID mismatch. Provided ID: {ProvidedId}, Product ID: {ProductId}", id, product.Id);
                return BadRequest();
            }

            await _productRepository.UpdateProductAsync(product);
            Log.Information("Updated product with ID {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            Log.Information("Deleted product with ID {Id}", id);
            return NoContent();
        }
    }
}
