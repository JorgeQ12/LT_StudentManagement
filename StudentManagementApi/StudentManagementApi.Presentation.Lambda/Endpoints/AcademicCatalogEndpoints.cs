using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.AcademicCatalog.GetActiveAcademicPrograms;
using StudentManagementApi.Application.Features.AcademicCatalog.GetActiveCoursesByAcademicProgram;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class AcademicCatalogEndpoints
{
    public static RouteGroupBuilder MapAcademicCatalogEndpoints(this RouteGroupBuilder api)
    {
        api.MapGroup("/AcademicPrograms")
            .WithTags("Academic Programs")
            .MapGet("/GetActiveAcademicPrograms", GetActiveAcademicPrograms)
            .Produces<IReadOnlyCollection<AcademicProgramResponse>>()
            .AllowAnonymous();

        api.MapGroup("/Courses")
            .WithTags("Courses")
            .MapGet("/GetActiveCoursesByAcademicProgram", GetActiveCoursesByAcademicProgram)
            .Produces<IReadOnlyCollection<CourseResponse>>()
            .ProducesValidationProblem()
            .AllowAnonymous();

        return api;
    }

    private static async Task<IResult> GetActiveAcademicPrograms(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetActiveAcademicProgramsQuery(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetActiveCoursesByAcademicProgram(
        Guid academicProgramId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetActiveCoursesByAcademicProgramQuery(academicProgramId), cancellationToken);
        return result.ToHttpResult(context);
    }
}
