namespace BankingSystemServices.Exceptions;

public class PropertyValidationException : Exception
{
    public string? ValidatedPropertiesName { get; set; }
    public string? ValidatedObjectName { get; set; }

    public PropertyValidationException()
    {
    }

    public PropertyValidationException(string message, string? validatedProperties, string? validatedObject) : base(message)
    {
        ValidatedPropertiesName = validatedProperties;
        ValidatedObjectName = validatedObject;
    }

    public PropertyValidationException(string message, Exception? inner, string? validatedProperties, string validatedObject) : base(message, inner)
    {
        ValidatedPropertiesName = validatedProperties;
        ValidatedObjectName = validatedObject;
    }
}