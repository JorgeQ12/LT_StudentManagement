using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CoursesCountSpec : Specification<Course>
{
    public CoursesCountSpec(string? search, CatalogStatus? status, AcademicProgramId? academicProgramId)
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
            .AsNoTracking();
    }
}
