using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.StudentProfiles.DeactivateCurrentStudentAccount;
using StudentManagementApi.Application.Features.StudentProfiles.GetCurrentStudentProfile;
using StudentManagementApi.Application.Features.StudentProfiles.UpdateCurrentStudentProfile;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.WebApi.Contracts.StudentProfiles;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Authorize(Roles = nameof(AccountRole.Student))]
[Route("api/[controller]")]
public sealed class StudentProfilesController(ISender sender) : ControllerBase
{
    /// <summary>Gets the profile of the currently authenticated student.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The current student profile.</returns>
    /// <response code="200">Returns the current student profile.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">The student was not found.</response>
    [HttpGet(nameof(GetCurrentStudentProfile))]
    public Task<Result<StudentResponse>> GetCurrentStudentProfile(CancellationToken cancellationToken) =>
        sender.Send(new GetCurrentStudentProfileQuery(), cancellationToken);

    /// <summary>Updates the profile of the currently authenticated student.</summary>
    /// <param name="request">The updated profile information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated student profile.</returns>
    /// <response code="200">The student profile was updated successfully.</response>
    /// <response code="400">The profile information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">The student was not found.</response>
    /// <response code="409">The email address is already registered.</response>
    [HttpPut(nameof(UpdateCurrentStudentProfile))]
    public Task<Result<StudentResponse>> UpdateCurrentStudentProfile(UpdateCurrentStudentProfileRequest request, CancellationToken cancellationToken) =>
        sender.Send(new UpdateCurrentStudentProfileCommand(request.FirstName, request.LastName, request.DateOfBirth, request.PhoneNumber, request.Email),
            cancellationToken);

    /// <summary>Deactivates the currently authenticated student account.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The student account was deactivated successfully.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not associated with a student.</response>
    /// <response code="404">The student was not found.</response>
    [HttpDelete(nameof(DeactivateCurrentStudentAccount))]
    public Task<Result<LogoutResponse>> DeactivateCurrentStudentAccount(CancellationToken cancellationToken) =>
        sender.Send(new DeactivateCurrentStudentAccountCommand(), cancellationToken);
}
