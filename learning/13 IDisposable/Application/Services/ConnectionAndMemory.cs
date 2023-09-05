using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Service;

public class ConnectionAndMemory : IDisposable
{
    public static long TotalFreed { get; private set; }
    public static long TotalAllocated { get; private set; }
    private readonly NpgsqlConnection _connection;
    private readonly IntPtr _chunkHandle;
    private readonly int _chunkSize;
    private bool _isFreed;
    
    public ConnectionAndMemory(int chunkSize, IConfiguration configuration)
    {
        var bankingSystemDb = new BankingSystemDb(configuration);
        _connection = bankingSystemDb.GetConnection();
        _connection.Open();
        _chunkSize = chunkSize;
        _chunkHandle = Marshal.AllocHGlobal(chunkSize);
        TotalAllocated += chunkSize;
    }
    private void ReleaseUnmanagedResources()
    {
        if (_isFreed) return;
        Marshal.FreeHGlobal(_chunkHandle);
        TotalFreed += _chunkSize;
        _isFreed = true;
    }
    public void DoWork() { }
    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _connection?.Dispose();
        }
    }
    public void Dispose()
    {
        Dispose(true);
    }
}