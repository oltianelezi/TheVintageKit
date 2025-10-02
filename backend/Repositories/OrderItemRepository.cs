using System;
using Npgsql;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Repositories;

public class OrderItemRepository
{
    private readonly string _connectionString;

    public OrderItemRepository()
    {
          _connectionString = Environment.GetEnvironmentVariable("SUPABASE_DB");
    }

    public async Task NewOrderItems(List<OrderItem> orderItems)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        foreach (var item in orderItems)
        {
            await using var command = new NpgsqlCommand(@"
            INSERT INTO orderitems (productid, orderid, quantity, size, unitprice)
            VALUES (@productid, @orderid, @quantity, @size, @unitprice);
        ", connection, transaction);

            command.Parameters.AddWithValue("@productid", item.ProductId);
            command.Parameters.AddWithValue("@orderid", item.OrderId);
            command.Parameters.AddWithValue("@quantity", item.Quantity);
            command.Parameters.AddWithValue("@size", item.Size);
            command.Parameters.AddWithValue("@unitprice", item.UnitPrice);

            await command.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }

}
