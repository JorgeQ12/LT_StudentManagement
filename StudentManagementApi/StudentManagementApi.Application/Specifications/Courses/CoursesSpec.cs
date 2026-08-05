using Ardalis.Specification;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CoursesSpec : Specification<Course>
{
    public CoursesSpec() =>
        Query.Include(course => course.TeachingAssignment!)
            .ThenInclude(assignment => assignment.Professor)
            .OrderBy(course => course.Code)
            .AsNoTracking();
}
