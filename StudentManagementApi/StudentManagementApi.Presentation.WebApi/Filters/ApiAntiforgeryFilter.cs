using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Presentation.WebApi.ProblemDetails;

namespace StudentManagementApi.Presentation.WebApi.Filters;

internal sealed class ApiAntiforgeryFilter(IAntiforgery antiforgery, IApiProblemDetailsFactory problemDetailsFactory) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (HttpMethods.IsGet(context.HttpContext.Request.Method) || HttpMethods.IsHead(context.HttpContext.Request.Method) ||
            HttpMethods.IsOptions(context.HttpContext.Request.Method) || HttpMethods.IsTrace(context.HttpContext.Request.Method)) return;

        try
        {
            await antiforgery.ValidateRequestAsync(context.HttpContext);
        }
        catch (AntiforgeryValidationException)
        {
            context.Result = new BadRequestObjectResult(problemDetailsFactory.Create(ErrorCode.InvalidAntiforgeryToken,
                StatusCodes.Status400BadRequest, context.HttpContext));
        }
    }
}
