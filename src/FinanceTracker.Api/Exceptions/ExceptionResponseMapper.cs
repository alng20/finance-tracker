using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Exceptions;

public class ExceptionResponseMapper : IExceptionResponseMapper
{
    public ProblemDetails Map(Exception exception, HttpContext context)
    {
        return exception switch
        {
            NotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = exception.Message,
                Instance = context.Request.Path,
            },

            ConflictException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = exception.Message,
                Instance = context.Request.Path,
            },

            AppValidationException validationException => new ValidationProblemDetails(
                validationException.Errors
            )
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Instance = context.Request.Path,
                Errors = validationException.Errors,
            },

            DomainException => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Domain rule error",
                Detail = exception.Message,
                Instance = context.Request.Path,
            },

            UnauthorizedException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "User is unauthorized",
                Detail = exception.Message,
                Instance = context.Request.Path,
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Detail = "An unexpected error occurred.",
                Instance = context.Request.Path,
            },
        };
    }
}
