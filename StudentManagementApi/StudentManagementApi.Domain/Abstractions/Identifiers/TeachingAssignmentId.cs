namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct TeachingAssignmentId(Guid Value)
{
    public static TeachingAssignmentId New() => new(Guid.NewGuid());
}
