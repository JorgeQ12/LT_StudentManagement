using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CoursesPageSpec : Specification<Course>
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

        Query.Include(course => course.TeachingAssignment!)
                .ThenInclude(assignment => assignment.Professor)
            .Where(course =>
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
            .AsNoTracking();
    }
}
