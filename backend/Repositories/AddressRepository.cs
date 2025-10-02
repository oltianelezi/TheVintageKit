using System;
using Npgsql;
using backend.Models;

namespace backend.Repositories;

public class AddressRepository
{
    private readonly string _connectionString;

    public AddressRepository()
    {
          _connectionString = Environment.GetEnvironmentVariable("SUPABASE_DB");
    }

    public async Task<int> CreateNewAddress(Address NewAddress)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = @"
                INSERT INTO addresses (firstname, lastname, city, country, state, street, zip)
                VALUES (@firstname, @lastname, @city, @country, @state, @street, @zip)
                RETURNING addressid;";

        command.Parameters.AddWithValue("@firstname", NewAddress.FirstName);
        command.Parameters.AddWithValue("@lastname", NewAddress.LastName);
        command.Parameters.AddWithValue("@city", NewAddress.City);
        command.Parameters.AddWithValue("@country", NewAddress.Country);
        command.Parameters.AddWithValue("@state", NewAddress.State);
        command.Parameters.AddWithValue("@street", NewAddress.Street);
        command.Parameters.AddWithValue("@zip", NewAddress.Zip);

        var newId = (long)await command.ExecuteScalarAsync(); // addressid is BIGINT in DB
        return (int)newId;
    }
}
