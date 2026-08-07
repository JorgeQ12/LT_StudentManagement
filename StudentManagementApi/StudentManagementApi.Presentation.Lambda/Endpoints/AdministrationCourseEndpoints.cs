using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.Courses.ActivateCourse;
using StudentManagementApi.Application.Features.Administration.Courses.AssignProfessor;
using StudentManagementApi.Application.Features.Administration.Courses.CreateCourse;
using StudentManagementApi.Application.Features.Administration.Courses.DeactivateCourse;
using StudentManagementApi.Application.Features.Administration.Courses.GetAllCourses;
using StudentManagementApi.Application.Features.Administration.Courses.GetCourseById;
using StudentManagementApi.Application.Features.Administration.Courses.UpdateCourse;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.Lambda.Contracts.Administration.Courses;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class AdministrationCourseEndpoints
{
    public static RouteGroupBuilder MapAdministrationCourseEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/AdministrationCourses")
            .WithTags("Administration - Courses")
            .RequireAuthorization(policy => policy.RequireRole(nameof(AccountRole.Administrator)));

        group.MapGet("/GetAllCourses", GetAllCourses)
            .Produces<PagedResponse<CourseResponse>>();
        group.MapGet("/GetCourseById", GetCourseById)
            .Produces<CourseResponse>();
        group.MapPost("/CreateCourse", CreateCourse)
            .Produces<CourseResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        group.MapPut("/UpdateCourse", UpdateCourse)
            .Produces<CourseResponse>()
            .ProducesValidationProblem();
        group.MapPut("/ActivateCourse", ActivateCourse)
            .Produces<CourseResponse>();
        group.MapDelete("/DeactivateCourse", DeactivateCourse)
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/AssignProfessor", AssignProfessor)
            .Produces<CourseResponse>()
            .ProducesValidationProblem();

        return group;
    }

    private static async Task<IResult> GetAllCourses(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CatalogStatus? status = null,
        Guid? academicProgramId = null)
    {
        var result = await sender.Send(
            new GetAllCoursesQuery(pageNumber, pageSize, search, status, academicProgramId),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetCourseById(
        Guid courseId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCourseByIdQuery(courseId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> CreateCourse(
        CreateCourseRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateCourseCommand(request.AcademicProgramId, request.Code, request.Name),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> UpdateCourse(
        UpdateCourseRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateCourseCommand(request.CourseId, request.Code, request.Name),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> ActivateCourse(
        ActivateCourseRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateCourseCommand(request.CourseId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> DeactivateCourse(
        Guid courseId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateCourseCommand(courseId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> AssignProfessor(
        AssignProfessorRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new AssignProfessorCommand(request.CourseId, request.ProfessorId),
            cancellationToken);
        return result.ToHttpResult(context);
    }
}
