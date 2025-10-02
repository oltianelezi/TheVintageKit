using backend.Models;
using Npgsql;

namespace backend.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository()
        {
            _connectionString = Environment.GetEnvironmentVariable("SUPABASE_DB");
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
                    Price = reader.GetDecimal(2),
                    Category = reader.GetString(3),
                    League = reader.GetString(4),
                    Description = reader.GetString(5),
                    ImageURL = reader.GetString(6),
                });
            }

            return Products;
        }


        public async Task<List<Product>>? GetProductsByLeague(string League)
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
                    Price = reader.GetDecimal(2),
                    Category = reader.GetString(3),
                    League = reader.GetString(4),
                    Description = reader.GetString(5),
                    ImageURL = reader.GetString(6),
                });
            }

            return Products;
        }

        public async Task<Product> GetProductById(int id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT productid, productname, price, category, league, description, imageurl 
                FROM Products 
                WHERE productid = @id";

            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                return new Product
                {
                    ProductID = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Category = reader.GetString(3),
                    League = reader.GetString(4),
                    Description = reader.GetString(5),
                    ImageURL = reader.GetString(6),
                };
            }

            return null;
        }
    }
}
