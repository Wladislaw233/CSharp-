using BankingSystemServices.Exceptions;

namespace BankingSystemServices.Services;

public static class ExceptionHandlingService
{
    public static string PropertyValidationExceptionHandler(PropertyValidationException exception,
        string? description = null)
    {
        description = DescriptionHandler(description ?? "Error when validating an object property.");

        var objectName = exception.ValidatedObjectName != null
            ? "\nObject: " + exception.ValidatedObjectName
            : "";

        var propertyName = exception.ValidatedPropertiesName != null
            ? "\nProperty: " + exception.ValidatedPropertiesName
            : "";

        description += objectName + propertyName;

        return GeneralExceptionHandler(exception, description, false);
    }

    public static string ArgumentExceptionHandler(ArgumentException exception, string? description = null)
    {
        description = DescriptionHandler(description ?? "The parameter was passed incorrectly.");

        var parameterName = exception.ParamName != null ? "Parameter name: " + exception.ParamName : "";

        description += parameterName;

        return GeneralExceptionHandler(exception, description, false);
    }

    public static string GeneralExceptionHandler<T>(T exception, string? description = null, bool processDesc = true)
        where T : Exception
    {
        description = processDesc ? DescriptionHandler(description) : description;
        var message = "\nMessage: " + exception.Message;
        var innerMessage = exception.InnerException != null ? "\nInner message: " + exception.InnerException : "";
        var stackTrace = exception.StackTrace != null ? "\nStack trace: " + exception.StackTrace : "";

        return description + message + innerMessage + stackTrace;
    }

    private static string DescriptionHandler(string? description)
    {
        var result = description != null && !string.IsNullOrWhiteSpace(description)
            ? description
            : "An unexpected error occurred.";
        return "Description: " + result;
    }
}