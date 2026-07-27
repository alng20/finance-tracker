using FinanceTracker.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Exceptions;

public interface IExceptionResponseMapper
{
    ProblemDetails Map(Exception exception, HttpContext context);
}
