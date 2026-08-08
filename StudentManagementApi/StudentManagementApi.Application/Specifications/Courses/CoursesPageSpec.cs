using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CoursesPageSpec : Specification<Course, CourseResponse>
{
    public CoursesPageSpec(
        int pageNumber,
        int pageSize,
        string? search,
        CatalogStatus? status,
        AcademicProgramId? academicProgramId)
    {
        var normalizedSearch = SearchValueObject.Normalize(search);
        var courseCode = SearchValueObject.TryCreate(normalizedSearch, CourseCode.Create);

        Query.Where(course =>
                (normalizedSearch == null
                    || (courseCode != null && course.Code == courseCode)
                    || course.Name.Contains(normalizedSearch)
                    || (course.TeachingAssignment != null
                        && course.TeachingAssignment.Professor != null
                        && (course.TeachingAssignment.Professor.FirstName + " "
                            + course.TeachingAssignment.Professor.LastName).Contains(normalizedSearch)))
                && (status == null || course.Status == status.Value)
                && (academicProgramId == null
                    || course.AcademicProgramId == academicProgramId.Value))
            .OrderBy(course => course.Code)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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
}
