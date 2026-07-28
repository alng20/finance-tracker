using FinanceTracker.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IExceptionResponseMapper _exceptionResponseMapper;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IExceptionResponseMapper exceptionResponseMapper
    )
    {
        _next = next;
        _logger = logger;
        _exceptionResponseMapper = exceptionResponseMapper;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            if (context.Response.HasStarted)
            {
                throw;
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = _exceptionResponseMapper.Map(exception, context);

        context.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
    }
}
