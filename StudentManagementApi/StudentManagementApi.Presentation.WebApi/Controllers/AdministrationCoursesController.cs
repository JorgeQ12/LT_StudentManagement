using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.Courses.ActivateCourse;
using StudentManagementApi.Application.Features.Administration.Courses.AssignProfessor;
using StudentManagementApi.Application.Features.Administration.Courses.CreateCourse;
using StudentManagementApi.Application.Features.Administration.Courses.DeactivateCourse;
using StudentManagementApi.Application.Features.Administration.Courses.GetAllCourses;
using StudentManagementApi.Application.Features.Administration.Courses.GetCourseById;
using StudentManagementApi.Application.Features.Administration.Courses.UpdateCourse;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.WebApi.Contracts.Administration.Courses;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Authorize(Roles = nameof(AccountRole.Administrator))]
[Route("api/[controller]")]
public sealed class AdministrationCoursesController(ISender sender) : ControllerBase
{
    /// <summary>Gets courses, including inactive courses, with paging and filters.</summary>
    /// <param name="request">The paging and filtering options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A page of courses.</returns>
    /// <response code="200">Returns the requested page of courses.</response>
    /// <response code="400">The paging or filtering options are invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    [HttpGet(nameof(GetAllCourses))]
    public Task<Result<PagedResponse<CourseResponse>>> GetAllCourses([FromQuery] GetAllCoursesRequest request, CancellationToken cancellationToken) =>
        sender.Send(new GetAllCoursesQuery(request.PageNumber, request.PageSize, request.Search, request.Status, request.AcademicProgramId), cancellationToken);

    /// <summary>Gets a course by identifier.</summary>
    /// <param name="request">The course identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested course.</returns>
    /// <response code="200">Returns the requested course.</response>
    /// <response code="400">The course identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The course was not found.</response>
    [HttpGet(nameof(GetCourseById))]
    public Task<Result<CourseResponse>> GetCourseById([FromQuery] GetCourseByIdRequest request, CancellationToken cancellationToken) =>
        sender.Send(new GetCourseByIdQuery(request.CourseId), cancellationToken);

    /// <summary>Creates a course in an academic program.</summary>
    /// <param name="request">The academic program and course information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created course.</returns>
    /// <response code="201">The course was created successfully.</response>
    /// <response code="400">The course information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The academic program was not found.</response>
    /// <response code="409">The course code is already registered.</response>
    [HttpPost(nameof(CreateCourse))]
    public Task<Result<CourseResponse>> CreateCourse(CreateCourseRequest request, CancellationToken cancellationToken) =>
        sender.Send(new CreateCourseCommand(request.AcademicProgramId, request.Code, request.Name), cancellationToken);

    /// <summary>Updates a course.</summary>
    /// <param name="request">The course identifier and updated information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated course.</returns>
    /// <response code="200">The course was updated successfully.</response>
    /// <response code="400">The course information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The course was not found.</response>
    /// <response code="409">The course is in use or its code is already registered.</response>
    [HttpPut(nameof(UpdateCourse))]
    public Task<Result<CourseResponse>> UpdateCourse(UpdateCourseRequest request, CancellationToken cancellationToken) =>
        sender.Send(new UpdateCourseCommand(request.CourseId, request.Code, request.Name), cancellationToken);

    /// <summary>Activates a course.</summary>
    /// <param name="request">The course identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The activated course.</returns>
    /// <response code="200">The course was activated successfully.</response>
    /// <response code="400">The course identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The course was not found.</response>
    [HttpPut(nameof(ActivateCourse))]
    public Task<Result<CourseResponse>> ActivateCourse(ActivateCourseRequest request, CancellationToken cancellationToken) =>
        sender.Send(new ActivateCourseCommand(request.CourseId), cancellationToken);

    /// <summary>Deactivates a course.</summary>
    /// <param name="request">The course identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The course was deactivated successfully.</response>
    /// <response code="400">The course identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The course was not found.</response>
    /// <response code="409">The course is used by an active enrollment.</response>
    [HttpDelete(nameof(DeactivateCourse))]
    public Task<Result<NoContentResponse>> DeactivateCourse([FromQuery] DeactivateCourseRequest request, CancellationToken cancellationToken) =>
        sender.Send(new DeactivateCourseCommand(request.CourseId), cancellationToken);

    /// <summary>Assigns or replaces the professor responsible for a course.</summary>
    /// <param name="request">The course and professor identifiers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The course with its teaching assignment.</returns>
    /// <response code="200">The professor was assigned successfully.</response>
    /// <response code="400">The course or professor identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The course or professor was not found.</response>
    /// <response code="409">The course is in use or the professor reached the teaching limit.</response>
    [HttpPut(nameof(AssignProfessor))]
    public Task<Result<CourseResponse>> AssignProfessor(AssignProfessorRequest request, CancellationToken cancellationToken) =>
        sender.Send(new AssignProfessorCommand(request.CourseId, request.ProfessorId), cancellationToken);
}
