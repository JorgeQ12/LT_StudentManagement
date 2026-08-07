using StudentManagementApi.Presentation.Lambda.Filters;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class LambdaEndpoints
{
    public static IEndpointRouteBuilder MapLambdaEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("/api").AddEndpointFilter<AntiforgeryEndpointFilter>();

        api.MapAuthenticationEndpoints();
        api.MapAcademicCatalogEndpoints();
        api.MapEnrollmentEndpoints();
        api.MapStudentProfileEndpoints();
        api.MapAdministrationAcademicProgramEndpoints();
        api.MapAdministrationCourseEndpoints();
        api.MapAdministrationProfessorEndpoints();
        api.MapAdministrationStudentEndpoints();

        return endpoints;
    }
}
