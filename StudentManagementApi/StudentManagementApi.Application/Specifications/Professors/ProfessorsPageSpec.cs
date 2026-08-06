using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorsPageSpec : Specification<Professor>
{
    public ProfessorsPageSpec(int pageNumber, int pageSize, string? search, CatalogStatus? status)
    {
        Query.Include(professor => professor.TeachingAssignments)
            .Where(professor =>
                (search == null
                    || professor.FirstName.Contains(search)
                    || professor.LastName.Contains(search)
                    || (professor.FirstName + " " + professor.LastName).Contains(search))
                && (status == null || professor.Status == status.Value))
            .OrderBy(professor => professor.LastName)
            .ThenBy(professor => professor.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
    }
}