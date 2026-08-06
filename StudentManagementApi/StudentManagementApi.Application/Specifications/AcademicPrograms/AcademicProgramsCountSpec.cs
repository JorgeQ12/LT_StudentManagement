using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Programs;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramsCountSpec : Specification<AcademicProgram>
{
    public AcademicProgramsCountSpec(string? search, CatalogStatus? status)
    {
        Query.Where(program =>
                (search == null
                    || program.Code.Value.Contains(search)
                    || program.Name.Contains(search)
                    || program.Description.Contains(search))
                && (status == null || program.Status == status.Value))
            .AsNoTracking();
    }
}