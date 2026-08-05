using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveEnrollmentsByCourseIdsSpec : Specification<Enrollment>
{
    public ActiveEnrollmentsByCourseIdsSpec(IReadOnlyCollection<CourseId> courseIds, StudentId excludedStudentId)
    {
        var ids = courseIds.ToArray();

        Query.Where(enrollment =>
                enrollment.Status == EnrollmentStatus.Active &&
                enrollment.StudentId != excludedStudentId &&
                enrollment.Courses.Any(item => ids.Contains(item.CourseId)))
            .Include(enrollment => enrollment.Student)
            .Include(enrollment => enrollment.Courses)
            .AsNoTracking();
    }
}
