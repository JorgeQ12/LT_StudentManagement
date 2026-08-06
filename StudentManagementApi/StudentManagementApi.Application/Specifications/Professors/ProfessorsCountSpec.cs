using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorsCountSpec : Specification<Professor>
{
    public ProfessorsCountSpec(string? search, CatalogStatus? status)
    {
        Query.Where(professor =>
                (search == null
                    || professor.FirstName.Contains(search)
                    || professor.LastName.Contains(search)
                    || (professor.FirstName + " " + professor.LastName).Contains(search))
                && (status == null || professor.Status == status.Value))
            .AsNoTracking();
    }
}