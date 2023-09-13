using System.Runtime.Serialization;

namespace BankingSystemServices.Exceptions;

[Serializable]
public class CustomException : Exception
{
    public string ParameterOfException { get; set; }

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
}