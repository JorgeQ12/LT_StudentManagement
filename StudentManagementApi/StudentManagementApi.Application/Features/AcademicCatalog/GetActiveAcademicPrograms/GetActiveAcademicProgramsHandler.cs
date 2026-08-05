using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Features.AcademicCatalog.GetActiveAcademicPrograms;

internal sealed class GetActiveAcademicProgramsHandler(IReadRepository<AcademicProgram> academicProgramRepository)
    : IRequestHandler<GetActiveAcademicProgramsQuery, Result<IReadOnlyCollection<AcademicProgramResponse>>>
{
    public async Task<Result<IReadOnlyCollection<AcademicProgramResponse>>> Handle(GetActiveAcademicProgramsQuery request, CancellationToken cancellationToken)
    {
        var programs = await academicProgramRepository.ListAsync(new ActiveAcademicProgramsSpec(), cancellationToken);

        return Result<IReadOnlyCollection<AcademicProgramResponse>>.Success(programs);
    }
}
