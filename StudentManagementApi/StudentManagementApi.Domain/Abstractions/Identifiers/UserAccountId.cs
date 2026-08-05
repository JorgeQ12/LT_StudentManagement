namespace StudentManagementApi.Domain.Abstractions;

public readonly record struct UserAccountId(Guid Value)
{
    public static UserAccountId New() => new(Guid.NewGuid());
}
