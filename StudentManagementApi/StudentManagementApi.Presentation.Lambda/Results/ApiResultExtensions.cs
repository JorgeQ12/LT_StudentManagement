using Ardalis.Result;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Infrastructure.Security.Cookies;
using StudentManagementApi.Presentation.Lambda.ProblemDetails;
using ArdalisResult = Ardalis.Result.IResult;
using HttpResult = Microsoft.AspNetCore.Http.IResult;

namespace StudentManagementApi.Presentation.Lambda.Results;

internal static class ApiResultExtensions
{
    public static HttpResult ToHttpResult(this ArdalisResult result, HttpContext httpContext) => result.Status switch
    {
        ResultStatus.Ok => Success(result, httpContext, StatusCodes.Status200OK),
        ResultStatus.Created => Success(result, httpContext, StatusCodes.Status201Created),
        ResultStatus.NoContent => NoContent(result, httpContext),
        ResultStatus.Invalid => Problem(httpContext.RequestServices.GetRequiredService<IApiProblemDetailsFactory>()
            .CreateValidation(result.ValidationErrors, httpContext)),
        ResultStatus.Unauthorized => Failure(result, httpContext, ErrorCode.Unauthorized, StatusCodes.Status401Unauthorized),
        ResultStatus.Forbidden => Failure(result, httpContext, ErrorCode.Forbidden, StatusCodes.Status403Forbidden),
        ResultStatus.NotFound => Failure(result, httpContext, ErrorCode.UnexpectedError, StatusCodes.Status404NotFound),
        ResultStatus.Conflict => Failure(result, httpContext, ErrorCode.ConcurrencyConflict, StatusCodes.Status409Conflict),
        ResultStatus.Unavailable => Failure(result, httpContext, ErrorCode.ServiceUnavailable, StatusCodes.Status503ServiceUnavailable),
        _ => Failure(result, httpContext, ErrorCode.UnexpectedError, StatusCodes.Status500InternalServerError)
    };

    private static HttpResult Success(ArdalisResult result, HttpContext httpContext, int statusCode)
    {
        var value = result.GetValue();
        if (value is AuthenticationSessionResponse session)
        {
            httpContext.Response.Cookies.Append(
                AuthenticationCookie.Name,
                session.AccessToken,
                AuthenticationCookie.CreateOptions(session.ExpiresAtUtc));
            value = session.User;
        }

        return Microsoft.AspNetCore.Http.Results.Json(value, statusCode: statusCode);
    }

    private static HttpResult NoContent(ArdalisResult result, HttpContext httpContext)
    {
        if (result.ValueType == typeof(LogoutResponse))
            httpContext.Response.Cookies.Delete(AuthenticationCookie.Name, AuthenticationCookie.CreateExpiredOptions());

        return Microsoft.AspNetCore.Http.Results.NoContent();
    }

    private static HttpResult Failure(ArdalisResult result, HttpContext httpContext, ErrorCode fallback, int statusCode)
    {
        var code = result.Errors
            .Select(error => Enum.TryParse<ErrorCode>(error, out var parsed) ? parsed : fallback)
            .FirstOrDefault(fallback);
        var details = httpContext.RequestServices.GetRequiredService<IApiProblemDetailsFactory>()
            .Create(code, statusCode, httpContext);
        return Problem(details);
    }

    private static HttpResult Problem(Microsoft.AspNetCore.Mvc.ProblemDetails details) =>
        Microsoft.AspNetCore.Http.Results.Problem(details);
}
