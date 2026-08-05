namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct EnrollmentId(Guid Value)
{
    public static EnrollmentId New() => new(Guid.NewGuid());
}
