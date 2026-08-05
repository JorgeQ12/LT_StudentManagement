using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.Professors.ActivateProfessor;
using StudentManagementApi.Application.Features.Administration.Professors.CreateProfessor;
using StudentManagementApi.Application.Features.Administration.Professors.DeactivateProfessor;
using StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;
using StudentManagementApi.Application.Features.Administration.Professors.GetProfessorById;
using StudentManagementApi.Application.Features.Administration.Professors.UpdateProfessor;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.WebApi.Contracts.Administration.Professors;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Authorize(Roles = nameof(AccountRole.Administrator))]
[Route("api/[controller]")]
public sealed class AdministrationProfessorsController(ISender sender) : ControllerBase
{
    /// <summary>Gets all professors, including inactive professors.</summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All professors.</returns>
    /// <response code="200">Returns all professors.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    [HttpGet(nameof(GetAllProfessors))]
    public Task<Result<IReadOnlyCollection<ProfessorResponse>>> GetAllProfessors(CancellationToken cancellationToken) =>
        sender.Send(new GetAllProfessorsQuery(), cancellationToken);

    /// <summary>Gets a professor by identifier.</summary>
    /// <param name="request">The professor identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested professor.</returns>
    /// <response code="200">Returns the requested professor.</response>
    /// <response code="400">The professor identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The professor was not found.</response>
    [HttpGet(nameof(GetProfessorById))]
    public Task<Result<ProfessorResponse>> GetProfessorById([FromQuery] GetProfessorByIdRequest request, CancellationToken cancellationToken) =>
        sender.Send(new GetProfessorByIdQuery(request.ProfessorId), cancellationToken);

    /// <summary>Creates a professor.</summary>
    /// <param name="request">The professor information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created professor.</returns>
    /// <response code="201">The professor was created successfully.</response>
    /// <response code="400">The professor information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    [HttpPost(nameof(CreateProfessor))]
    public Task<Result<ProfessorResponse>> CreateProfessor(CreateProfessorRequest request, CancellationToken cancellationToken) =>
        sender.Send(new CreateProfessorCommand(request.FirstName, request.LastName), cancellationToken);

    /// <summary>Updates a professor.</summary>
    /// <param name="request">The professor identifier and updated information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated professor.</returns>
    /// <response code="200">The professor was updated successfully.</response>
    /// <response code="400">The professor information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The professor was not found.</response>
    [HttpPut(nameof(UpdateProfessor))]
    public Task<Result<ProfessorResponse>> UpdateProfessor(UpdateProfessorRequest request, CancellationToken cancellationToken) =>
        sender.Send(new UpdateProfessorCommand(request.ProfessorId, request.FirstName, request.LastName), cancellationToken);

    /// <summary>Activates a professor.</summary>
    /// <param name="request">The professor identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The activated professor.</returns>
    /// <response code="200">The professor was activated successfully.</response>
    /// <response code="400">The professor identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The professor was not found.</response>
    [HttpPut(nameof(ActivateProfessor))]
    public Task<Result<ProfessorResponse>> ActivateProfessor(ActivateProfessorRequest request, CancellationToken cancellationToken) =>
        sender.Send(new ActivateProfessorCommand(request.ProfessorId), cancellationToken);

    /// <summary>Deactivates a professor.</summary>
    /// <param name="request">The professor identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The professor was deactivated successfully.</response>
    /// <response code="400">The professor identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The professor was not found.</response>
    [HttpDelete(nameof(DeactivateProfessor))]
    public Task<Result<NoContentResponse>> DeactivateProfessor([FromQuery] DeactivateProfessorRequest request, CancellationToken cancellationToken) =>
        sender.Send(new DeactivateProfessorCommand(request.ProfessorId), cancellationToken);
}
