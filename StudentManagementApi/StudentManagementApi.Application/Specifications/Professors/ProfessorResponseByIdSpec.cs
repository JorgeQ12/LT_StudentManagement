using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorResponseByIdSpec : SingleResultSpecification<Professor, ProfessorResponse>
{
    public ProfessorResponseByIdSpec(ProfessorId professorId) =>
        Query.Where(professor => professor.Id == professorId)
            .AsNoTracking()
            .Select(professor => new ProfessorResponse(
                professor.Id.Value,
                professor.FirstName,
                professor.LastName,
                professor.Status,
                professor.TeachingAssignments.Count));
}
