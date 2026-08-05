using StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;
using StudentManagementApi.Presentation.WebApi.Cors;
using StudentManagementApi.Presentation.WebApi.ProblemDetails;

namespace StudentManagementApi.Presentation.WebApi;

public static class WebApplicationExtensions
{
    public static async Task UseWebApiAsync(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages(ApiStatusCodeHandler.HandleAsync);
        app.UseHttpsRedirection();
        app.UseCors(ApiCorsPolicies.Frontend);
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.SwaggerEndpoint("/openapi/v1.json", "Student Management API v1");
                options.DocumentTitle = "Student Management API";
            });
        }

        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            await using var scope = app.Services.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<AdministratorBootstrapper>().CreateIfConfiguredAsync();
        }
    }
}
