using MediatR;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.ActivateAcademicProgram;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.CreateAcademicProgram;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.DeactivateAcademicProgram;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAcademicProgramById;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAllAcademicPrograms;
using StudentManagementApi.Application.Features.Administration.AcademicPrograms.UpdateAcademicProgram;
using StudentManagementApi.Domain;
using StudentManagementApi.Presentation.Lambda.Contracts.Administration.AcademicPrograms;
using StudentManagementApi.Presentation.Lambda.Results;

namespace StudentManagementApi.Presentation.Lambda.Endpoints;

internal static class AdministrationAcademicProgramEndpoints
{
    public static RouteGroupBuilder MapAdministrationAcademicProgramEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/AdministrationAcademicPrograms")
            .WithTags("Administration - Academic Programs")
            .RequireAuthorization(policy => policy.RequireRole(nameof(AccountRole.Administrator)));

        group.MapGet("/GetAllAcademicPrograms", GetAllAcademicPrograms)
            .Produces<PagedResponse<AcademicProgramResponse>>();
        group.MapGet("/GetAcademicProgramById", GetAcademicProgramById)
            .Produces<AcademicProgramResponse>();
        group.MapPost("/CreateAcademicProgram", CreateAcademicProgram)
            .Produces<AcademicProgramResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
        group.MapPut("/UpdateAcademicProgram", UpdateAcademicProgram)
            .Produces<AcademicProgramResponse>()
            .ProducesValidationProblem();
        group.MapPut("/ActivateAcademicProgram", ActivateAcademicProgram)
            .Produces<AcademicProgramResponse>();
        group.MapDelete("/DeactivateAcademicProgram", DeactivateAcademicProgram)
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    private static async Task<IResult> GetAllAcademicPrograms(
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CatalogStatus? status = null)
    {
        var result = await sender.Send(
            new GetAllAcademicProgramsQuery(pageNumber, pageSize, search, status),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> GetAcademicProgramById(
        Guid academicProgramId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAcademicProgramByIdQuery(academicProgramId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> CreateAcademicProgram(
        CreateAcademicProgramRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateAcademicProgramCommand(request.Code, request.Name, request.Description),
            cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> UpdateAcademicProgram(
        UpdateAcademicProgramRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateAcademicProgramCommand(
            request.AcademicProgramId,
            request.Code,
            request.Name,
            request.Description), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> ActivateAcademicProgram(
        ActivateAcademicProgramRequest request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateAcademicProgramCommand(request.AcademicProgramId), cancellationToken);
        return result.ToHttpResult(context);
    }

    private static async Task<IResult> DeactivateAcademicProgram(
        Guid academicProgramId,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateAcademicProgramCommand(academicProgramId), cancellationToken);
        return result.ToHttpResult(context);
    }
}
