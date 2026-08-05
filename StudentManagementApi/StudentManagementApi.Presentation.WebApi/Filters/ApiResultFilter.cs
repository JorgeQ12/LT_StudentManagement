using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Infrastructure.Security.Cookies;
using StudentManagementApi.Presentation.WebApi.ProblemDetails;
using ArdalisResult = Ardalis.Result.IResult;

namespace StudentManagementApi.Presentation.WebApi.Filters;

internal sealed class ApiResultFilter(IApiProblemDetailsFactory problemDetailsFactory) : IAsyncResultFilter
{
    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: ArdalisResult result }) context.Result = Translate(result, context.HttpContext);
        return next();
    }

    private IActionResult Translate(ArdalisResult result, HttpContext httpContext) => result.Status switch
    {
        ResultStatus.Ok => Success(result, httpContext, StatusCodes.Status200OK),
        ResultStatus.Created => Success(result, httpContext, StatusCodes.Status201Created),
        ResultStatus.NoContent => NoContent(result, httpContext),
        ResultStatus.Invalid => Problem(problemDetailsFactory.CreateValidation(result.ValidationErrors, httpContext)),
        ResultStatus.Unauthorized => Failure(result, httpContext, ErrorCode.Unauthorized, StatusCodes.Status401Unauthorized),
        ResultStatus.Forbidden => Failure(result, httpContext, ErrorCode.Forbidden, StatusCodes.Status403Forbidden),
        ResultStatus.NotFound => Failure(result, httpContext, ErrorCode.UnexpectedError, StatusCodes.Status404NotFound),
        ResultStatus.Conflict => Failure(result, httpContext, ErrorCode.ConcurrencyConflict, StatusCodes.Status409Conflict),
        ResultStatus.Unavailable => Failure(result, httpContext, ErrorCode.ServiceUnavailable, StatusCodes.Status503ServiceUnavailable),
        _ => Failure(result, httpContext, ErrorCode.UnexpectedError, StatusCodes.Status500InternalServerError)
    };

    private static ObjectResult Success(ArdalisResult result, HttpContext httpContext, int statusCode)
    {
        var value = result.GetValue();
        if (value is AuthenticationSessionResponse session)
        {
            httpContext.Response.Cookies.Append(AuthenticationCookie.Name, session.AccessToken, AuthenticationCookie.CreateOptions(session.ExpiresAtUtc));
            value = session.User;
        }

        return new ObjectResult(value) { StatusCode = statusCode };
    }

    private static StatusCodeResult NoContent(ArdalisResult result, HttpContext httpContext)
    {
        if (result.ValueType == typeof(LogoutResponse))
            httpContext.Response.Cookies.Delete(AuthenticationCookie.Name, AuthenticationCookie.CreateExpiredOptions());
        return new StatusCodeResult(StatusCodes.Status204NoContent);
    }

    private ObjectResult Failure(ArdalisResult result, HttpContext httpContext, ErrorCode fallback, int statusCode)
    {
        var code = result.Errors.Select(error => Enum.TryParse<ErrorCode>(error, out var parsed) ? parsed : fallback).FirstOrDefault(fallback);
        return Problem(problemDetailsFactory.Create(code, statusCode, httpContext));
    }

    private static ObjectResult Problem(Microsoft.AspNetCore.Mvc.ProblemDetails details) => new(details) { StatusCode = details.Status };
}
