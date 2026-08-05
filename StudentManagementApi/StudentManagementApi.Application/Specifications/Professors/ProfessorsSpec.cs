using Ardalis.Specification;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorsSpec : Specification<Professor>
{
    public ProfessorsSpec() =>
        Query.Include(professor => professor.TeachingAssignments)
            .OrderBy(professor => professor.LastName)
            .ThenBy(professor => professor.FirstName)
            .AsNoTracking();
}
