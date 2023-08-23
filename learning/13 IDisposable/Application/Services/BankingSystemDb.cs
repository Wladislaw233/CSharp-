using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Service;

public class BankingSystemDb
{
    private readonly IConfiguration _configuration;
    private int _connectionsCounter;

    public BankingSystemDb(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection GetConnection()
    {
        var connectionString = _configuration.GetConnectionString("Postgres");
        var connection = new NpgsqlConnection(connectionString);
        return connection;
    }

    private void OpenConnection()
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            LogOpenedConnectionCount();
        }
    }

    private void LogOpenedConnectionCount()
    {
        _connectionsCounter++;
        Console.WriteLine(_connectionsCounter);
    }

    public void StartOpenConnections()
    {
        for (var i = 0; i < 200; i++) OpenConnection();
    }

    public void CreateConnectionsAndMemory(int count)
    {
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var chunkSize = random.Next(4096);
            using (var connectionAndMemory = new ConnectionAndMemory(chunkSize, _configuration))
            {
                connectionAndMemory.DoWork();
            }
        }
    }
}