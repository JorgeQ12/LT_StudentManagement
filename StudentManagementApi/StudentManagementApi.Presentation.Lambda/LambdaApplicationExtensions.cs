using StudentManagementApi.Presentation.Lambda.Cors;
using StudentManagementApi.Presentation.Lambda.Endpoints;
using StudentManagementApi.Presentation.Lambda.ProblemDetails;

namespace StudentManagementApi.Presentation.Lambda;

internal static class LambdaApplicationExtensions
{
    public static WebApplication UseLambdaPresentation(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages(ApiStatusCodeHandler.HandleAsync);
        app.UseHttpsRedirection();
        app.UseCors(ApiCorsPolicies.Frontend);
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("../openapi/v1.json", "Student Management API v1");
            options.DocumentTitle = "Student Management API";
        });
        app.MapLambdaEndpoints();
        return app;
    }
}
