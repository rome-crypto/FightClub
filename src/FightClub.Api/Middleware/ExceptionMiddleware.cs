using System.Text.Json;
using FightClub.Application.Exceptions;

namespace FightClub.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                NotFoundException => 404,
                BusinessException => 400,
                _ => 500
            };

            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message
            });
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        if (ex is AppException appEx)
        {
            context.Response.StatusCode = appEx.StatusCode;

            var response = new
            {
                error = appEx.GetType().Name,
                message = appEx.Message
            };
            
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        
        context.Response.StatusCode = 500;

        var fallback = new
        {
            Error = "ServerError",
            Message = "Unexpected error",
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(fallback));
    }
}
