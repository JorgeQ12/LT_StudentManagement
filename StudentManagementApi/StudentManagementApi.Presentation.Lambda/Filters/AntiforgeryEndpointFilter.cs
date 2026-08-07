using Microsoft.AspNetCore.Antiforgery;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Presentation.Lambda.ProblemDetails;

namespace StudentManagementApi.Presentation.Lambda.Filters;

internal sealed class AntiforgeryEndpointFilter(
    IAntiforgery antiforgery,
    IApiProblemDetailsFactory problemDetailsFactory) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var method = context.HttpContext.Request.Method;
        if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method) || HttpMethods.IsOptions(method) || HttpMethods.IsTrace(method))
            return await next(context);

        try
        {
            await antiforgery.ValidateRequestAsync(context.HttpContext);
            return await next(context);
        }
        catch (AntiforgeryValidationException)
        {
            var details = problemDetailsFactory.Create(
                ErrorCode.InvalidAntiforgeryToken,
                StatusCodes.Status400BadRequest,
                context.HttpContext);
            return Microsoft.AspNetCore.Http.Results.Problem(details);
        }
    }
}
