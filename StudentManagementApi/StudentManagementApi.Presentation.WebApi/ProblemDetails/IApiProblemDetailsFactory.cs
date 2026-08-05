using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Presentation.WebApi.ProblemDetails;

public interface IApiProblemDetailsFactory
{
    Microsoft.AspNetCore.Mvc.ProblemDetails Create(ErrorCode code, int statusCode, HttpContext httpContext);
    ValidationProblemDetails CreateValidation(IEnumerable<ValidationError> errors, HttpContext httpContext);
}
