using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramResponseByIdSpec
    : SingleResultSpecification<AcademicProgram, AcademicProgramResponse>
{
    public AcademicProgramResponseByIdSpec(AcademicProgramId academicProgramId) =>
        Query.Where(program => program.Id == academicProgramId)
            .AsNoTracking()
            .Select(program => new AcademicProgramResponse(program.Id.Value, program.Code.Value, program.Name, program.Description, program.Status));
}
