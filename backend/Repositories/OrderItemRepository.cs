using System;
using Npgsql;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Repositories;

public class OrderItemRepository
{
    private readonly string _connectionString;

    public OrderItemRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("SupabaseDb");
    }

    public async Task NewOrderItems(List<OrderItem> Order)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        foreach (OrderItem item in Order)
        {
            await using var command = connection.CreateCommand();

            command.CommandText = @"
            INSERT INTO orderitem (productid, orderid, quantity, size, unitprice)
            VALUES (@productid, @orderid, @quantity, @size, @unitprice) 
            ";

            command.Parameters.AddWithValue("@productid", item.ProductId);
            command.Parameters.AddWithValue("@orderid", item.OrderId);
            command.Parameters.AddWithValue("@quantity", item.Quantity);
            command.Parameters.AddWithValue("@size", item.Size);
            command.Parameters.AddWithValue("@unitprice", item.UnitPrice);

            await command.ExecuteNonQueryAsync();
        }
    }
}
