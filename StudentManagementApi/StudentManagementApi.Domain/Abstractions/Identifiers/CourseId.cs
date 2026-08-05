namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct CourseId(Guid Value)
{
    public static CourseId New() => new(Guid.NewGuid());
}
