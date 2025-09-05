using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }


        [HttpGet("league/{League}")]
        public async Task<IActionResult> GetProductByLeague(string League)
        {
            var products = await _productRepository.GetProductsByLeague(League);
            return Ok(products);
        }
    }
}
