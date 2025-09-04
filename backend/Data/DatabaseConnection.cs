using Npgsql;
using System.Data;

public class DatabaseConnection
{
    private readonly IConfiguration _config;

    public DatabaseConnection(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_config.GetConnectionString("SupabaseDb"));
    }
}
