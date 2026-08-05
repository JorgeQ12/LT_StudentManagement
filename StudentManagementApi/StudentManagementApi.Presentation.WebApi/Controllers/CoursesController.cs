using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.AcademicCatalog.GetActiveCoursesByAcademicProgram;
using StudentManagementApi.Presentation.WebApi.Contracts.AcademicCatalog;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public sealed class CoursesController(ISender sender) : ControllerBase
{
    /// <summary>Gets the active courses that belong to an academic program.</summary>
    /// <param name="request">The academic program identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The active courses in the academic program.</returns>
    /// <response code="200">Returns the active courses.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="404">The academic program was not found.</response>
    [HttpGet(nameof(GetActiveCoursesByAcademicProgram))]
    public Task<Result<IReadOnlyCollection<CourseResponse>>> GetActiveCoursesByAcademicProgram(
        [FromQuery] GetActiveCoursesByAcademicProgramRequest request, CancellationToken cancellationToken) =>
        sender.Send(new GetActiveCoursesByAcademicProgramQuery(request.AcademicProgramId), cancellationToken);
}
