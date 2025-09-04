using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        // Inject repository through constructor
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: /product
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            System.Console.WriteLine(products);
            return Ok(products);
        }
    }
}
