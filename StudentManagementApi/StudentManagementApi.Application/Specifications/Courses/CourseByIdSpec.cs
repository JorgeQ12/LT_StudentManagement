using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CourseByIdSpec : SingleResultSpecification<Course>
{
    public CourseByIdSpec(CourseId courseId) =>
        Query.Where(course => course.Id == courseId)
            .Include(course => course.TeachingAssignment!)
            .ThenInclude(assignment => assignment.Professor);
}
