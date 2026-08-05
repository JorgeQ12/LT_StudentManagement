using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveCoursesByProgramSpec : Specification<Course, CourseResponse>
{
    public ActiveCoursesByProgramSpec(AcademicProgramId academicProgramId) =>
        Query.Where(course =>
                course.AcademicProgramId == academicProgramId &&
                course.Status == CatalogStatus.Active &&
                course.TeachingAssignment != null &&
                course.TeachingAssignment.Professor.Status == CatalogStatus.Active)
            .OrderBy(course => course.Code)
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
