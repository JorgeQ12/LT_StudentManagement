using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAllAcademicPrograms;

internal sealed class GetAllAcademicProgramsHandler(IReadRepository<AcademicProgram> academicProgramRepository)
    : IRequestHandler<GetAllAcademicProgramsQuery, Result<PagedResponse<AcademicProgramResponse>>>
{
    public async Task<Result<PagedResponse<AcademicProgramResponse>>> Handle(
        GetAllAcademicProgramsQuery request,
        CancellationToken cancellationToken)
    {
        var search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();

        var totalCount = await academicProgramRepository.CountAsync(
            new AcademicProgramsCountSpec(search, request.Status),
            cancellationToken);
        var programs = await academicProgramRepository.ListAsync(
            new AcademicProgramsPageSpec(request.PageNumber, request.PageSize, search, request.Status),
            cancellationToken);

        return Result<PagedResponse<AcademicProgramResponse>>.Success(new(
            programs.Select(program => program.ToResponse()).ToArray(),
            request.PageNumber,
            request.PageSize,
            totalCount));
    }
}
