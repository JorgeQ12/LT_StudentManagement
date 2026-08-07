using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Enrollments.CancelCurrentStudentEnrollment;
using StudentManagementApi.Application.Features.Enrollments.CreateCurrentStudentEnrollment;
using StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentClassmatesByCourse;
using StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentEnrollment;
using StudentManagementApi.Application.Features.Enrollments.ReplaceCurrentStudentSelectedCourses;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.Lambda.Contracts.Enrollments;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class EnrollmentEndpoints
{
    public static RouteGroupBuilder MapEnrollmentEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/Enrollments")
            .WithTags("Enrollments")
            .RequireAuthorization(policy => policy.RequireRole(nameof(AccountRole.Student)));

        group.MapPost("/CreateCurrentStudentEnrollment", CreateCurrentStudentEnrollment)
            .Produces<EnrollmentResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        group.MapGet("/GetCurrentStudentEnrollment", GetCurrentStudentEnrollment)
            .Produces<EnrollmentResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPut("/ReplaceCurrentStudentSelectedCourses", ReplaceCurrentStudentSelectedCourses)
            .Produces<EnrollmentResponse>()
            .ProducesValidationProblem();
        group.MapDelete("/CancelCurrentStudentEnrollment", CancelCurrentStudentEnrollment)
            .Produces(StatusCodes.Status204NoContent);
        group.MapGet("/GetCurrentStudentClassmatesByCourse", GetCurrentStudentClassmatesByCourse)
            .Produces<IReadOnlyCollection<ClassmatesByCourseResponse>>();

        return group;
    }

    private static async Task<IResult> CreateCurrentStudentEnrollment(
        CreateCurrentStudentEnrollmentRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateCurrentStudentEnrollmentCommand(request.AcademicProgramId, request.CourseIds),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetCurrentStudentEnrollment(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCurrentStudentEnrollmentQuery(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> ReplaceCurrentStudentSelectedCourses(
        ReplaceSelectedCoursesRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ReplaceCurrentStudentSelectedCoursesCommand(request.CourseIds), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> CancelCurrentStudentEnrollment(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelCurrentStudentEnrollmentCommand(), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetCurrentStudentClassmatesByCourse(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCurrentStudentClassmatesByCourseQuery(), cancellationToken);
        return result.ToHttpResult(context);
    }
}
