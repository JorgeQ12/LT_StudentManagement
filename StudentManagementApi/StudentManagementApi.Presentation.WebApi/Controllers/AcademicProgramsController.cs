using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.AcademicCatalog.GetActiveAcademicPrograms;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public sealed class AcademicProgramsController(ISender sender) : ControllerBase
{
    /// <summary>Gets all active academic programs.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The active academic programs.</returns>
    /// <response code="200">Returns the active academic programs.</response>
    [HttpGet(nameof(GetActiveAcademicPrograms))]
    public Task<Result<IReadOnlyCollection<AcademicProgramResponse>>> GetActiveAcademicPrograms(CancellationToken cancellationToken) =>
        sender.Send(new GetActiveAcademicProgramsQuery(), cancellationToken);
}
