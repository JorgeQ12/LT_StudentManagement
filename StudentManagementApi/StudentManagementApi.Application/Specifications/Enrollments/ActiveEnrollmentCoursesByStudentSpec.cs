using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveEnrollmentCoursesByStudentSpec
    : SingleResultSpecification<Enrollment, CurrentEnrollmentCoursesProjection>
{
    public ActiveEnrollmentCoursesByStudentSpec(StudentId studentId) =>
        Query.Where(enrollment =>
                enrollment.StudentId == studentId && enrollment.Status == EnrollmentStatus.Active)
            .AsNoTracking()
            .Select(enrollment => new CurrentEnrollmentCoursesProjection(
                enrollment.Courses
                    .OrderBy(item => item.Course.Code)
                    .Select(item => new CurrentEnrollmentCourseProjection(
                        item.CourseId.Value,
                        item.Course.Name))
                    .ToArray()));
}
