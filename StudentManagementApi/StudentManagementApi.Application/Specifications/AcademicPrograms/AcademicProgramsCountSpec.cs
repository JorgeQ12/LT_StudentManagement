using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramsCountSpec : Specification<AcademicProgram>
{
    public AcademicProgramsCountSpec(string? search, CatalogStatus? status)
    {
        var normalizedSearch = SearchValueObject.Normalize(search);
        var programCode = SearchValueObject.TryCreate(normalizedSearch, ProgramCode.Create);

        Query.Where(program =>
                (normalizedSearch == null
                    || (programCode != null && program.Code == programCode)
                    || program.Name.Contains(normalizedSearch)
                    || program.Description.Contains(normalizedSearch))
                && (status == null || program.Status == status.Value))
            .AsNoTracking();
    }
}
