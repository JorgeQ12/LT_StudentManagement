using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ProfessorByCourseIdSpec : SingleResultSpecification<Professor>
{
    public ProfessorByCourseIdSpec(CourseId courseId) =>
        Query.Where(professor => professor.TeachingAssignments.Any(assignment => assignment.CourseId == courseId))
            .Include(professor => professor.TeachingAssignments);
}
