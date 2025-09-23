using System;
using backend.Models;
using Npgsql;

namespace backend.Repositories;

public class MessageRepository
{
    private readonly string _connectionString;

    public MessageRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("SupabaseDb");
    }

    public async Task CreateMessage(Message NewMessage)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = @"
        INSERT into messages(name, email, comment)
        VALUES (@name, @email, @comment)
        ";

        command.Parameters.AddWithValue("@name", NewMessage.Name);
        command.Parameters.AddWithValue("@email", NewMessage.Email);
        command.Parameters.AddWithValue("@comment", NewMessage.Comment);

        await command.ExecuteNonQueryAsync();
    }
}
