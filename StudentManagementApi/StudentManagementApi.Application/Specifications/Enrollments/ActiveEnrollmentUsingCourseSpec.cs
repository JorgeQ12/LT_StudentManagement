using Ardalis.Specification;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Specifications;

internal sealed class ActiveEnrollmentUsingCourseSpec : SingleResultSpecification<Enrollment>
{
    public ActiveEnrollmentUsingCourseSpec(CourseId courseId) =>
        Query.Where(enrollment => enrollment.Status == EnrollmentStatus.Active && enrollment.Courses.Any(item => item.CourseId == courseId));
}
