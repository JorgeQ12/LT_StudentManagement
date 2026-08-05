namespace StudentManagementApi.Presentation.WebApi.Antiforgery;

public interface IAntiforgeryTokenService
{
    AntiforgeryTokenResponse Generate(HttpContext httpContext);
}
