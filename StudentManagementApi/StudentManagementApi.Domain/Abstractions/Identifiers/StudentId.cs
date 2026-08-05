namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct StudentId(Guid Value)
{
    public static StudentId New() => new(Guid.NewGuid());
}
