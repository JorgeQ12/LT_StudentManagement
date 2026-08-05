using Ardalis.Specification;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramsSpec : Specification<AcademicProgram>
{
    public AcademicProgramsSpec() => Query.OrderBy(program => program.Name).AsNoTracking();
}
