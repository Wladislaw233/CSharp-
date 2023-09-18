using System.Net;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models.DTO;

namespace BankAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exc)
        {
            await ExceptionHandleAsync(httpContext, exc);
        }
    }

    private async Task ExceptionHandleAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => HttpStatusCode.BadRequest,
            ValueNotFoundException => HttpStatusCode.NotFound,
            PropertyValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var excMsg = exception.ToString();
        
        _logger.LogError(excMsg);
        
        var response = httpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;
        
        var errorDetails = new ErrorDetailsDto
        {
            StatusCode = (int)statusCode,
            ErrorMessage = excMsg
        };
        
        await response.WriteAsJsonAsync(errorDetails.ToString());
    }
}