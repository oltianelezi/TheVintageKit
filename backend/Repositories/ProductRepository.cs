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
            command.CommandText = "SELECT productid, productname, price, category, league, description, imageurl FROM Products;";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Products.Add(new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Price = reader.GetFloat(2),
                    Category = reader.GetString(3),
                    League = reader.GetString(4),
                    Description = reader.GetString(5),
                    ImageURL = reader.GetString(6),
                });
            }

            return Products;
        }


        public async Task<List<Product>> GetProductsByLeague(string League)
        {
            var Products = new List<Product>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT productid, productname, price, category, league, description, imageurl 
                FROM Products 
                WHERE league = @league;";

            command.Parameters.AddWithValue("@league", League);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Products.Add(new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Price = reader.GetFloat(2),
                    Category = reader.GetString(3),
                    League = reader.GetString(4),
                    Description = reader.GetString(5),
                    ImageURL = reader.GetString(6),
                });
            }

            return Products;
        }
    }
}
