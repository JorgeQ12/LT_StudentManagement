namespace StudentManagementApi.Application.Specifications;

internal sealed record CurrentEnrollmentCoursesProjection(
    IReadOnlyCollection<CurrentEnrollmentCourseProjection> Courses);

internal sealed record CurrentEnrollmentCourseProjection(Guid CourseId, string CourseName);

internal sealed record ClassmateEnrollmentProjection(
    string FullName,
    IReadOnlyCollection<Guid> CourseIds);
