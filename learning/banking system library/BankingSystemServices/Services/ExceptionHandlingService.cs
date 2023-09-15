using BankingSystemServices.Exceptions;

namespace BankingSystemServices.Services;

public static class ExceptionHandlingService
{
    public static string GeneralExceptionHandler<T>(T exception, string? description = null, bool processDesc = true)
        where T : Exception
    {
        description = description != null ? "Description: " + description : "";
        var message = "\nMessage: " + exception.Message;
        var innerMessage = exception.InnerException != null ? "\nInner message: " + exception.InnerException : "";
        var stackTrace = exception.StackTrace != null ? "\nStack trace: " + exception.StackTrace : "";

        return description + message + innerMessage + stackTrace;
    }
}