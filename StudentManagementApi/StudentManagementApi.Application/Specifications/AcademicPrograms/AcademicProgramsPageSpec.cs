using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AcademicProgramsPageSpec : Specification<AcademicProgram, AcademicProgramResponse>
{
    public AcademicProgramsPageSpec(int pageNumber, int pageSize, string? search, CatalogStatus? status)
    {
        var normalizedSearch = SearchValueObject.Normalize(search);
        var programCode = SearchValueObject.TryCreate(normalizedSearch, ProgramCode.Create);

        Query.Where(program =>
                (normalizedSearch == null
                    || (programCode != null && program.Code == programCode)
                    || program.Name.Contains(normalizedSearch)
                    || program.Description.Contains(normalizedSearch))
                && (status == null || program.Status == status.Value))
            .OrderBy(program => program.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Select(program => new AcademicProgramResponse(
                program.Id.Value,
                program.Code.Value,
                program.Name,
                program.Description,
                program.Status));
    }
}
