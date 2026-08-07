using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Presentation.Lambda.ProblemDetails;

internal sealed partial class ApiExceptionHandler(IApiProblemDetailsFactory problemDetailsFactory, ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var concurrencyConflict = exception is DbUpdateConcurrencyException;
        var invalidRequest = exception is BadHttpRequestException;
        var code = concurrencyConflict
            ? ErrorCode.ConcurrencyConflict
            : invalidRequest ? ErrorCode.ValidationFailed : ErrorCode.UnexpectedError;
        var statusCode = concurrencyConflict
            ? StatusCodes.Status409Conflict
            : invalidRequest ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;
        LogUnhandledException(logger, exception, httpContext.TraceIdentifier);
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetailsFactory.Create(code, statusCode, httpContext), cancellationToken);
        return true;
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Unhandled request exception with trace identifier {TraceIdentifier}")]
    private static partial void LogUnhandledException(ILogger logger, Exception exception, string traceIdentifier);
}
