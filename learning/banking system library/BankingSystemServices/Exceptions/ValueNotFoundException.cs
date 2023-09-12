namespace BankingSystemServices.Exceptions;

public class ValueNotFoundException : Exception
{
    public ValueNotFoundException()
    {
    }

    public ValueNotFoundException(string message) : base(message)
    {
    }

    public ValueNotFoundException(string message, Exception? inner) : base(message, inner)
    {
    }
}