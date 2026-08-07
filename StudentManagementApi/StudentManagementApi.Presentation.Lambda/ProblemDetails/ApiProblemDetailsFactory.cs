using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Presentation.Lambda.ProblemDetails;

internal sealed class ApiProblemDetailsFactory(IErrorMessageProvider messageProvider) : IApiProblemDetailsFactory
{
    private const string RequestIdentifier = "request";

    public Microsoft.AspNetCore.Mvc.ProblemDetails Create(ErrorCode code, int statusCode, HttpContext httpContext)
    {
        var message = messageProvider.Resolve(code);
        var details = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = message.Title,
            Status = statusCode,
            Detail = message.Detail
        };
        AddExtensions(details, code, httpContext.TraceIdentifier);
        return details;
    }

    public ValidationProblemDetails CreateValidation(IEnumerable<ValidationError> errors, HttpContext httpContext)
    {
        var validationErrors = errors.ToArray();
        var message = messageProvider.Resolve(ErrorCode.ValidationFailed);
        var groupedMessages = validationErrors.GroupBy(error => NormalizeIdentifier(error.Identifier)).ToDictionary(group => group.Key,
            group => group.Select(error => messageProvider.Resolve(Parse(error.ErrorCode, ErrorCode.ValidationFailed)).Detail).ToArray());
        var details = new ValidationProblemDetails(groupedMessages)
        {
            Title = message.Title,
            Status = StatusCodes.Status400BadRequest,
            Detail = message.Detail
        };
        AddExtensions(details, ErrorCode.ValidationFailed, httpContext.TraceIdentifier);
        return details;
    }

    private static ErrorCode Parse(string? value, ErrorCode fallback) => Enum.TryParse<ErrorCode>(value, out var code) ? code : fallback;
    private static string NormalizeIdentifier(string? identifier) => string.IsNullOrWhiteSpace(identifier) ? RequestIdentifier : identifier;

    private static void AddExtensions(Microsoft.AspNetCore.Mvc.ProblemDetails details, ErrorCode code, string traceIdentifier)
    {
        details.Extensions["code"] = code.ToString();
        details.Extensions["traceId"] = traceIdentifier;
    }
}
