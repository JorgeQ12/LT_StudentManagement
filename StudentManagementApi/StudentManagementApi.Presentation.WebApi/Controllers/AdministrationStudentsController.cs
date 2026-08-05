using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.Students.ActivateStudent;
using StudentManagementApi.Application.Features.Administration.Students.CreateStudent;
using StudentManagementApi.Application.Features.Administration.Students.DeactivateStudent;
using StudentManagementApi.Application.Features.Administration.Students.GetAllStudents;
using StudentManagementApi.Application.Features.Administration.Students.GetStudentById;
using StudentManagementApi.Application.Features.Administration.Students.UpdateStudent;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.WebApi.Contracts.Administration.Students;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Authorize(Roles = nameof(AccountRole.Administrator))]
[Route("api/[controller]")]
public sealed class AdministrationStudentsController(ISender sender) : ControllerBase
{
    /// <summary>Gets a paged collection of students.</summary>
    /// <param name="request">The pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A page of students.</returns>
    /// <response code="200">Returns the requested page of students.</response>
    /// <response code="400">The pagination parameters are invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    [HttpGet(nameof(GetAllStudents))]
    public Task<Result<PagedResponse<StudentResponse>>> GetAllStudents([FromQuery] GetAllStudentsRequest request,
        CancellationToken cancellationToken) => sender.Send(new GetAllStudentsQuery(request.PageNumber, request.PageSize), cancellationToken);

    /// <summary>Gets a student by identifier.</summary>
    /// <param name="request">The student identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested student.</returns>
    /// <response code="200">Returns the requested student.</response>
    /// <response code="400">The student identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The student was not found.</response>
    [HttpGet(nameof(GetStudentById))]
    public Task<Result<StudentResponse>> GetStudentById([FromQuery] GetStudentByIdRequest request, CancellationToken cancellationToken) =>
        sender.Send(new GetStudentByIdQuery(request.StudentId), cancellationToken);

    /// <summary>Creates a student account.</summary>
    /// <param name="request">The student and account information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created student.</returns>
    /// <response code="201">The student was created successfully.</response>
    /// <response code="400">The student information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="409">The email address or document number is already registered.</response>
    [HttpPost(nameof(CreateStudent))]
    public Task<Result<StudentResponse>> CreateStudent(CreateStudentRequest request, CancellationToken cancellationToken) =>
        sender.Send(new CreateStudentCommand(request.FirstName, request.LastName, request.DocumentNumber, request.DateOfBirth, request.PhoneNumber,
            request.Email, request.Password), cancellationToken);

    /// <summary>Updates a student and its account information.</summary>
    /// <param name="request">The student identifier and updated information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated student.</returns>
    /// <response code="200">The student was updated successfully.</response>
    /// <response code="400">The student information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The student was not found.</response>
    /// <response code="409">The email address or document number is already registered.</response>
    [HttpPut(nameof(UpdateStudent))]
    public Task<Result<StudentResponse>> UpdateStudent(UpdateStudentRequest request, CancellationToken cancellationToken) =>
        sender.Send(new UpdateStudentCommand(request.StudentId, request.FirstName, request.LastName, request.DocumentNumber, request.DateOfBirth,
            request.PhoneNumber, request.Email), cancellationToken);

    /// <summary>Activates a student account.</summary>
    /// <param name="request">The student identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The activated student.</returns>
    /// <response code="200">The student account was activated successfully.</response>
    /// <response code="400">The student identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The student was not found.</response>
    [HttpPut(nameof(ActivateStudent))]
    public Task<Result<StudentResponse>> ActivateStudent(ActivateStudentRequest request, CancellationToken cancellationToken) =>
        sender.Send(new ActivateStudentCommand(request.StudentId), cancellationToken);

    /// <summary>Deactivates a student account.</summary>
    /// <param name="request">The student identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The student account was deactivated successfully.</response>
    /// <response code="400">The student identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The student was not found.</response>
    [HttpDelete(nameof(DeactivateStudent))]
    public Task<Result<NoContentResponse>> DeactivateStudent([FromQuery] DeactivateStudentRequest request, CancellationToken cancellationToken) =>
        sender.Send(new DeactivateStudentCommand(request.StudentId), cancellationToken);
}
