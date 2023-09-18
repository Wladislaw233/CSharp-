namespace BankAPI.Middlewares;

public static class ExceptionHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}