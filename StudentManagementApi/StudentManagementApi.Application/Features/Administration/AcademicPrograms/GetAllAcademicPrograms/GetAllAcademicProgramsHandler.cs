using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.Administration.AcademicPrograms.GetAllAcademicPrograms;

internal sealed class GetAllAcademicProgramsHandler(IReadRepository<AcademicProgram> academicProgramRepository)
    : IRequestHandler<GetAllAcademicProgramsQuery, Result<IReadOnlyCollection<AcademicProgramResponse>>>
{
    public async Task<Result<IReadOnlyCollection<AcademicProgramResponse>>> Handle(GetAllAcademicProgramsQuery request, CancellationToken cancellationToken)
    {
        var programs = await academicProgramRepository.ListAsync(new AcademicProgramsSpec(), cancellationToken);
        return Result<IReadOnlyCollection<AcademicProgramResponse>>.Success(programs.Select(program => program.ToResponse()).ToArray());
    }
}
