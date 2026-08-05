namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct ProfessorId(Guid Value)
{
    public static ProfessorId New() => new(Guid.NewGuid());
}
