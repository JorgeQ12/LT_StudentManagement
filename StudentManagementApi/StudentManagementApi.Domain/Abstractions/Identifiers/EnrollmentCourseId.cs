namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct EnrollmentCourseId(Guid Value)
{
    public static EnrollmentCourseId New() => new(Guid.NewGuid());
}
