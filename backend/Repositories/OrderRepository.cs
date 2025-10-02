using System;
using backend.Models;
using Npgsql;

namespace backend.Repositories;

public class OrderRepository
{
    private readonly string _connectionString;

    public OrderRepository()
    {
          _connectionString = Environment.GetEnvironmentVariable("SUPABASE_DB");
    }

    public async Task<int> CreateOrder(Order NewOrder)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = @"
        INSERT into orders(totalprice, email, addressid)
        VALUES (@totalprice, @email, @addressid)
        RETURNING orderid;
        ";

        command.Parameters.AddWithValue("@totalprice", NewOrder.TotalPrice);
        command.Parameters.AddWithValue("@email", NewOrder.Email);
        command.Parameters.AddWithValue("@addressid", NewOrder.AddressId);

        var newId = (long)await command.ExecuteScalarAsync();
        return (int)newId;
    }

}
