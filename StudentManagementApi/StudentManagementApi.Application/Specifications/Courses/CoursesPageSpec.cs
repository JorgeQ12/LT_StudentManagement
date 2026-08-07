using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

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
        Query.Include(course => course.TeachingAssignment!)
                .ThenInclude(assignment => assignment.Professor)
            .Where(course =>
                (search == null
                    || course.Code.Value.Contains(search)
                    || course.Name.Contains(search)
                    || (course.TeachingAssignment != null
                        && course.TeachingAssignment.Professor != null
                        && (course.TeachingAssignment.Professor.FirstName + " "
                            + course.TeachingAssignment.Professor.LastName).Contains(search)))
                && (status == null || course.Status == status.Value)
                && (academicProgramId == null
                    || course.AcademicProgramId == academicProgramId.Value))
            .OrderBy(course => course.Code)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();
    }
}
