using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorsPageSpec : Specification<Professor, ProfessorResponse>
{
    public ProfessorsPageSpec(int pageNumber, int pageSize, string? search, CatalogStatus? status)
    {
        Query.Where(professor =>
                (search == null
                    || professor.FirstName.Contains(search)
                    || professor.LastName.Contains(search)
                    || (professor.FirstName + " " + professor.LastName).Contains(search))
                && (status == null || professor.Status == status.Value))
            .OrderBy(professor => professor.LastName)
            .ThenBy(professor => professor.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Select(professor => new ProfessorResponse(
                professor.Id.Value,
                professor.FirstName,
                professor.LastName,
                professor.Status,
                professor.TeachingAssignments.Count));
    }
}
