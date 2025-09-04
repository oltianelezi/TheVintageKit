using backend.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SupabaseDb");
        }

        public async Task<List<Product>> GetProducts()
        {
            var Products = new List<Product>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT ProductID, ProductName, Price FROM Products;";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Products.Add(new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Price = reader.GetFloat(2)
                });
            }

            return Products;
        }
    }
}
