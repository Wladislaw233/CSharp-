namespace BankingSystemServices.Exceptions;

[Serializable]
public class ValueNotFoundException : Exception
{
    public ValueNotFoundException()
    {
    }

    public ValueNotFoundException(string message) : base(message)
    {
    }

    public ValueNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}