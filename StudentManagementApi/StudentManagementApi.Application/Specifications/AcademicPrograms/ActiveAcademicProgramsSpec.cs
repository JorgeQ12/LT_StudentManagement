using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveAcademicProgramsSpec : Specification<AcademicProgram, AcademicProgramResponse>
{
    public ActiveAcademicProgramsSpec() =>
        Query.Where(program => program.Status == CatalogStatus.Active)
            .OrderBy(program => program.Name)
            .AsNoTracking()
            .Select(program => new AcademicProgramResponse(program.Id.Value, program.Code.Value, program.Name, program.Description, program.Status));
}
