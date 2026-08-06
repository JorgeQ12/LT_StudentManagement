using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.ActivateAcademicProgram;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.CreateAcademicProgram;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.DeactivateAcademicProgram;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAcademicProgramById;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAllAcademicPrograms;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.UpdateAcademicProgram;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.WebApi.Contracts.Administration.AcademicPrograms;

namespace StudentManagementApi.Presentation.WebApi.Controllers;

[ApiController]
[Authorize(Roles = nameof(AccountRole.Administrator))]
[Route("api/[controller]")]
public sealed class AdministrationAcademicProgramsController(ISender sender) : ControllerBase
{
    /// <summary>Gets academic programs, including inactive programs, with paging and filters.</summary>
    /// <param name="request">The paging and filtering options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A page of academic programs.</returns>
    /// <response code="200">Returns the requested page of academic programs.</response>
    /// <response code="400">The paging or filtering options are invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    [HttpGet(nameof(GetAllAcademicPrograms))]
    public Task<Result<PagedResponse<AcademicProgramResponse>>> GetAllAcademicPrograms([FromQuery] GetAllAcademicProgramsRequest request,
        CancellationToken cancellationToken) => sender.Send(new GetAllAcademicProgramsQuery(request.PageNumber, request.PageSize, request.Search, request.Status), cancellationToken);

    /// <summary>Gets an academic program by identifier.</summary>
    /// <param name="request">The academic program identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The requested academic program.</returns>
    /// <response code="200">Returns the requested academic program.</response>
    /// <response code="400">The academic program identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The academic program was not found.</response>
    [HttpGet(nameof(GetAcademicProgramById))]
    public Task<Result<AcademicProgramResponse>> GetAcademicProgramById([FromQuery] GetAcademicProgramByIdRequest request,
        CancellationToken cancellationToken) => sender.Send(new GetAcademicProgramByIdQuery(request.AcademicProgramId), cancellationToken);

    /// <summary>Creates an academic program.</summary>
    /// <param name="request">The academic program information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created academic program.</returns>
    /// <response code="201">The academic program was created successfully.</response>
    /// <response code="400">The academic program information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="409">The academic program code is already registered.</response>
    [HttpPost(nameof(CreateAcademicProgram))]
    public Task<Result<AcademicProgramResponse>> CreateAcademicProgram(CreateAcademicProgramRequest request, CancellationToken cancellationToken) =>
        sender.Send(new CreateAcademicProgramCommand(request.Code, request.Name, request.Description), cancellationToken);

    /// <summary>Updates an academic program.</summary>
    /// <param name="request">The academic program identifier and updated information.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated academic program.</returns>
    /// <response code="200">The academic program was updated successfully.</response>
    /// <response code="400">The academic program information is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The academic program was not found.</response>
    /// <response code="409">The academic program code is already registered.</response>
    [HttpPut(nameof(UpdateAcademicProgram))]
    public Task<Result<AcademicProgramResponse>> UpdateAcademicProgram(UpdateAcademicProgramRequest request, CancellationToken cancellationToken) =>
        sender.Send(new UpdateAcademicProgramCommand(request.AcademicProgramId, request.Code, request.Name, request.Description), cancellationToken);

    /// <summary>Activates an academic program.</summary>
    /// <param name="request">The academic program identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The activated academic program.</returns>
    /// <response code="200">The academic program was activated successfully.</response>
    /// <response code="400">The academic program identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The academic program was not found.</response>
    [HttpPut(nameof(ActivateAcademicProgram))]
    public Task<Result<AcademicProgramResponse>> ActivateAcademicProgram(ActivateAcademicProgramRequest request,
        CancellationToken cancellationToken) => sender.Send(new ActivateAcademicProgramCommand(request.AcademicProgramId), cancellationToken);

    /// <summary>Deactivates an academic program.</summary>
    /// <param name="request">The academic program identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">The academic program was deactivated successfully.</response>
    /// <response code="400">The academic program identifier is invalid.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated account is not an administrator.</response>
    /// <response code="404">The academic program was not found.</response>
    /// <response code="409">The academic program is used by an active enrollment.</response>
    [HttpDelete(nameof(DeactivateAcademicProgram))]
    public Task<Result<NoContentResponse>> DeactivateAcademicProgram([FromQuery] DeactivateAcademicProgramRequest request,
        CancellationToken cancellationToken) => sender.Send(new DeactivateAcademicProgramCommand(request.AcademicProgramId), cancellationToken);
}
