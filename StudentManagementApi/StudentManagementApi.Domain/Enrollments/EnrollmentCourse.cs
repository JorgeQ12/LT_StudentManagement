using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Domain.Enrollments;

public sealed class EnrollmentCourse : Entity<EnrollmentCourseId>
{
    private EnrollmentCourse()
        : base(default) { }

    private EnrollmentCourse(EnrollmentCourseId id, EnrollmentId enrollmentId, CourseId courseId)
        : base(id)
    {
        EnrollmentId = enrollmentId;
        CourseId = courseId;
    }

    public EnrollmentId EnrollmentId { get; private set; }
    public CourseId CourseId { get; private set; }
    public Enrollment Enrollment { get; private set; } = null!;
    public Course Course { get; private set; } = null!;

    internal static EnrollmentCourse Create(EnrollmentCourseId id, EnrollmentId enrollmentId, CourseId courseId) =>
        new(id, enrollmentId, courseId);
}
