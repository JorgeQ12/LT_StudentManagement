using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CourseResponseByIdSpec : SingleResultSpecification<Course, CourseResponse>
{
    public CourseResponseByIdSpec(CourseId courseId) =>
        Query.Where(course => course.Id == courseId)
            .AsNoTracking()
            .Select(course => new CourseResponse(
                course.Id.Value,
                course.AcademicProgramId.Value,
                course.Code.Value,
                course.Name,
                course.Credits,
                course.Status,
                course.TeachingAssignment == null
                    ? null
                    : course.TeachingAssignment.ProfessorId.Value,
                course.TeachingAssignment == null
                    ? null
                    : course.TeachingAssignment.Professor.FirstName + " " +
                      course.TeachingAssignment.Professor.LastName));
}
