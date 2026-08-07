namespace StudentManagementApi.Presentation.Lambda.Antiforgery;

public interface IAntiforgeryTokenService
{
    AntiforgeryTokenResponse Generate(HttpContext httpContext);
}
