namespace BankingSystemServices.Exceptions;

[Serializable]
public class DatabaseNotConnectedException : Exception
{

    public DatabaseNotConnectedException()
    {
    }
    
    public DatabaseNotConnectedException(string message) : base(message)
    { 
    }
    
    public DatabaseNotConnectedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}