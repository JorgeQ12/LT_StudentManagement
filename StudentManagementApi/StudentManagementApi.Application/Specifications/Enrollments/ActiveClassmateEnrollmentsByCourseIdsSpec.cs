using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveClassmateEnrollmentsByCourseIdsSpec
    : Specification<Enrollment, ClassmateEnrollmentProjection>
{
    public ActiveClassmateEnrollmentsByCourseIdsSpec(
        IReadOnlyCollection<CourseId> courseIds,
        StudentId excludedStudentId)
    {
        var ids = courseIds.ToArray();

        Query.Where(enrollment =>
                enrollment.Status == EnrollmentStatus.Active &&
                enrollment.StudentId != excludedStudentId &&
                enrollment.Courses.Any(item => ids.Contains(item.CourseId)))
            .AsNoTracking()
            .Select(enrollment => new ClassmateEnrollmentProjection(
                enrollment.Student.FirstName + " " + enrollment.Student.LastName,
                enrollment.Courses
                    .Where(item => ids.Contains(item.CourseId))
                    .Select(item => item.CourseId.Value)
                    .ToArray()));
    }
}
