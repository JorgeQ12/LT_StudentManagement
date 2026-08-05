using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramByIdSpec : SingleResultSpecification<AcademicProgram>
{
    public AcademicProgramByIdSpec(AcademicProgramId id) => Query.Where(program => program.Id == id);
}
