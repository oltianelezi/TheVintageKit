using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all products
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // Get products by league
        public async Task<List<Product>> GetProductsByLeague(string league)
        {
            return await _context.Products
                                 .Where(p => p.League == league)
                                 .ToListAsync();
        }

        // Get product by ID
        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
                                 .FirstOrDefaultAsync(p => p.ProductID == id);
        }
    }
}
