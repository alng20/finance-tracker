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

            ValidationAppException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = exception.Message,
                Instance = context.Request.Path,
            },

            DomainException => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Domain rule error",
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
