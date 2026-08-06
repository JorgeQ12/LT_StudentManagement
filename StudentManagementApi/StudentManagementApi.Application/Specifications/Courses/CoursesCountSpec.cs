using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Application.Specifications;

internal sealed class CoursesCountSpec : Specification<Course>
{
    public CoursesCountSpec(string? search, CatalogStatus? status, AcademicProgramId? academicProgramId)
    {
        Query.Where(course =>
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
            .AsNoTracking();
    }
}
