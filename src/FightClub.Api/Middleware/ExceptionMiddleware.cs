using FightClub.Application.Exceptions;
using FightClub.Domain.Entities;
using System.Text.Json;

namespace FightClub.Api.Middleware;

/// <summary>
/// Middleware для исключений
/// </summary>
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
                AppException => 400,
                _ => 500
            };

            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message
            });
        }
    }
}
