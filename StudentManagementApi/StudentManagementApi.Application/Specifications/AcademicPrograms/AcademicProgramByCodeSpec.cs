using Ardalis.Specification;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramByCodeSpec : SingleResultSpecification<AcademicProgram>
{
    public AcademicProgramByCodeSpec(string code)
    {
        var normalizedCode = ProgramCode.Create(code);
        Query.Where(program => program.Code == normalizedCode);
    }
}
