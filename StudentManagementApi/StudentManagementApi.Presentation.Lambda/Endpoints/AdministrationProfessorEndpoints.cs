using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.Professors.ActivateProfessor;
using StudentManagementApi.Application.Features.Administration.Professors.CreateProfessor;
using StudentManagementApi.Application.Features.Administration.Professors.DeactivateProfessor;
using StudentManagementApi.Application.Features.Administration.Professors.GetAllProfessors;
using StudentManagementApi.Application.Features.Administration.Professors.GetProfessorById;
using StudentManagementApi.Application.Features.Administration.Professors.UpdateProfessor;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.Lambda.Contracts.Administration.Professors;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class AdministrationProfessorEndpoints
{
    public static RouteGroupBuilder MapAdministrationProfessorEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/AdministrationProfessors")
            .WithTags("Administration - Professors")
            .RequireAuthorization(policy => policy.RequireRole(nameof(AccountRole.Administrator)));

        group.MapGet("/GetAllProfessors", GetAllProfessors)
            .Produces<PagedResponse<ProfessorResponse>>();
        group.MapGet("/GetProfessorById", GetProfessorById)
            .Produces<ProfessorResponse>();
        group.MapPost("/CreateProfessor", CreateProfessor)
            .Produces<ProfessorResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        group.MapPut("/UpdateProfessor", UpdateProfessor)
            .Produces<ProfessorResponse>()
            .ProducesValidationProblem();
        group.MapPut("/ActivateProfessor", ActivateProfessor)
            .Produces<ProfessorResponse>();
        group.MapDelete("/DeactivateProfessor", DeactivateProfessor)
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    private static async Task<IResult> GetAllProfessors(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CatalogStatus? status = null)
    {
        var result = await sender.Send(
            new GetAllProfessorsQuery(pageNumber, pageSize, search, status),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetProfessorById(
        Guid professorId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProfessorByIdQuery(professorId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> CreateProfessor(
        CreateProfessorRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateProfessorCommand(request.FirstName, request.LastName),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> UpdateProfessor(
        UpdateProfessorRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UpdateProfessorCommand(request.ProfessorId, request.FirstName, request.LastName),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> ActivateProfessor(
        ActivateProfessorRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateProfessorCommand(request.ProfessorId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> DeactivateProfessor(
        Guid professorId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateProfessorCommand(professorId), cancellationToken);
        return result.ToHttpResult(context);
    }
}
