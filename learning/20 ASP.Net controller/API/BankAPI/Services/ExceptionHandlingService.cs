using BankingSystemServices.Exceptions;

namespace Services;

public class ExceptionHandlingService
{
    public static string CustomExceptionHandling(CustomException exception, string description)
    {
        var mess = description
                   + "\nInfo: \n"
                   + exception.Message
                   + (string.IsNullOrWhiteSpace(exception.ParameterOfException)
                       ? ""
                       : $" Parameter: {exception.ParameterOfException}")
                   + "\nStackTrace:"
                   + exception.StackTrace;

        return mess;
    }

    public static string ExceptionHandling(Exception exception)
    {
        var mess = "An unexpected error occurred. "
                   + "\nInfo: \n"
                   + exception.Message
                   + "\nStackTrace:"
                   + exception.StackTrace;
        
        return mess;
    }
}