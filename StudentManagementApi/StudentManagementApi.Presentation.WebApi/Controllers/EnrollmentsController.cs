using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Enrollments.CancelCurrentStudentEnrollment;
using StudentManagementApi.Application.Features.Enrollments.CreateCurrentStudentEnrollment;
using StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentClassmatesByCourse;
using StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentEnrollment;
using StudentManagementApi.Application.Features.Enrollments.ReplaceCurrentStudentSelectedCourses;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.WebApi.Contracts.Enrollments;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Authorize(Roles = nameof(AccountRole.Student))]
[Route("api/[controller]")]
public sealed class EnrollmentsController(ISender sender) : ControllerBase
{
    /// <summary>Creates the enrollment of the currently authenticated student.</summary>
    /// <param name="request">The academic program and selected courses.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created enrollment.</returns>
    /// <response code="201">The enrollment was created successfully.</response>
    /// <response code="400">The selected courses violate an enrollment rule.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">The academic program or a selected course was not found.</response>
    /// <response code="409">The student already has an active enrollment.</response>
    [HttpPost(nameof(CreateCurrentStudentEnrollment))]
    public Task<Result<EnrollmentResponse>> CreateCurrentStudentEnrollment(CreateCurrentStudentEnrollmentRequest request,
        CancellationToken cancellationToken) => sender.Send(
        new CreateCurrentStudentEnrollmentCommand(request.AcademicProgramId, request.CourseIds), cancellationToken);

    /// <summary>Gets the active enrollment of the currently authenticated student.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The current student enrollment.</returns>
    /// <response code="200">Returns the current student enrollment.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">An active enrollment was not found.</response>
    [HttpGet(nameof(GetCurrentStudentEnrollment))]
    public Task<Result<EnrollmentResponse>> GetCurrentStudentEnrollment(CancellationToken cancellationToken) =>
        sender.Send(new GetCurrentStudentEnrollmentQuery(), cancellationToken);

    /// <summary>Replaces the courses selected by the currently authenticated student.</summary>
    /// <param name="request">The replacement course identifiers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated enrollment.</returns>
    /// <response code="200">The selected courses were replaced successfully.</response>
    /// <response code="400">The selected courses violate an enrollment rule.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">The enrollment, academic program, or a selected course was not found.</response>
    [HttpPut(nameof(ReplaceCurrentStudentSelectedCourses))]
    public Task<Result<EnrollmentResponse>> ReplaceCurrentStudentSelectedCourses(ReplaceSelectedCoursesRequest request,
        CancellationToken cancellationToken) => sender.Send(new ReplaceCurrentStudentSelectedCoursesCommand(request.CourseIds), cancellationToken);

    /// <summary>Cancels the enrollment of the currently authenticated student.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The enrollment was cancelled successfully.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">An active enrollment was not found.</response>
    [HttpDelete(nameof(CancelCurrentStudentEnrollment))]
    public Task<Result<NoContentResponse>> CancelCurrentStudentEnrollment(CancellationToken cancellationToken) =>
        sender.Send(new CancelCurrentStudentEnrollmentCommand(), cancellationToken);

    /// <summary>Gets the classmates grouped by course for the currently authenticated student.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The classmates for each selected course.</returns>
    /// <response code="200">Returns the classmates grouped by course.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">An active enrollment was not found.</response>
    [HttpGet(nameof(GetCurrentStudentClassmatesByCourse))]
    public Task<Result<IReadOnlyCollection<ClassmatesByCourseResponse>>> GetCurrentStudentClassmatesByCourse(CancellationToken cancellationToken) =>
        sender.Send(new GetCurrentStudentClassmatesByCourseQuery(), cancellationToken);
}
