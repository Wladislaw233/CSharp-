using System.Runtime.Serialization;

namespace BankingSystemServices.Exceptions;

[Serializable]
public class CustomException : Exception
{
    public string ParameterOfException;

    public CustomException(string parameterOfException)
    {
        ParameterOfException = parameterOfException;
    }

    public CustomException(string message, string parameterOfException) : base(message)
    {
        ParameterOfException = parameterOfException;
    }

    public CustomException(string message, Exception? inner, string parameterOfException) : base(message, inner)
    {
        ParameterOfException = parameterOfException;
    }

    protected CustomException(SerializationInfo info, StreamingContext context, string parameterOfException) :
        base(info, context)
    {
        ParameterOfException = parameterOfException;
    }

    public static void ExceptionHandling(string description, CustomException exception)
    {
        Console.WriteLine(description + exception.Message);
        Console.WriteLine(string.IsNullOrWhiteSpace(exception.ParameterOfException)
            ? ""
            : $"Параметр: {exception.ParameterOfException}");
    }
}