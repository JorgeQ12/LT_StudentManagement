using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CoursesByIdsSpec : Specification<Course>
{
    public CoursesByIdsSpec(IReadOnlyCollection<CourseId> courseIds)
    {
        var ids = courseIds.ToArray();

        Query.Where(course => ids.Contains(course.Id))
            .Include(course => course.TeachingAssignment!)
            .ThenInclude(assignment => assignment.Professor);
    }
}
