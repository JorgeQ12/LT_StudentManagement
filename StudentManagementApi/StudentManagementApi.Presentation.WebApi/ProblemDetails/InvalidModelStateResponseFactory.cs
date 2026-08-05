using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Errors;

namespace StudentManagementApi.Presentation.WebApi.ProblemDetails;

internal sealed class InvalidModelStateResponseFactory(IApiProblemDetailsFactory problemDetailsFactory)
{
    public IActionResult Create(ActionContext context)
    {
        var errors = context.ModelState.SelectMany(entry => entry.Value?.Errors.Select(_ => new ValidationError(entry.Key, string.Empty,
            ErrorCode.ValidationFailed.ToString(), ValidationSeverity.Error)) ?? []).ToArray();
        return new BadRequestObjectResult(problemDetailsFactory.CreateValidation(errors, context.HttpContext));
    }
}
