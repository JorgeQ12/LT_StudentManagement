using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorByIdSpec : SingleResultSpecification<Professor>
{
    public ProfessorByIdSpec(ProfessorId professorId) =>
        Query.Where(professor => professor.Id == professorId).Include(professor => professor.TeachingAssignments);
}
