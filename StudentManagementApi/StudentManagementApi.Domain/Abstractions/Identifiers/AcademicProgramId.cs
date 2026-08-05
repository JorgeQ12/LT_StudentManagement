namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct AcademicProgramId(Guid Value)
{
    public static AcademicProgramId New() => new(Guid.NewGuid());
}
