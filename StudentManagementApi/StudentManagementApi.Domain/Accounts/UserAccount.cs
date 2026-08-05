using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Domain.Accounts;

public sealed class UserAccount : AggregateRoot<UserAccountId>
{
    private UserAccount()
        : base(default) { }

    private UserAccount(UserAccountId id, Email email, string passwordHash, AccountRole role, StudentId? studentId, DateTimeOffset createdAtUtc)
        : base(id)
    {
        Email = email;
        PasswordHash = Require(passwordHash);
        Role = role;
        StudentId = studentId;
        Status = AccountStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public AccountRole Role { get; private set; }
    public AccountStatus Status { get; private set; }
    public StudentId? StudentId { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static UserAccount CreateStudent(
        UserAccountId id,
        StudentId studentId,
        Email email,
        string passwordHash,
        DateTimeOffset createdAtUtc
    ) => new(id, email, passwordHash, AccountRole.Student, studentId, createdAtUtc);

    public static UserAccount CreateAdministrator(UserAccountId id, Email email, string passwordHash, DateTimeOffset createdAtUtc) =>
        new(id, email, passwordHash, AccountRole.Administrator, null, createdAtUtc);

    public void UpdateEmail(Email email, DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        Email = email;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void ChangePasswordHash(string passwordHash, DateTimeOffset updatedAtUtc)
    {
        PasswordHash = Require(passwordHash);
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Activate(DateTimeOffset updatedAtUtc)
    {
        Status = AccountStatus.Active;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Deactivate(DateTimeOffset updatedAtUtc)
    {
        Status = AccountStatus.Inactive;
        UpdatedAtUtc = updatedAtUtc;
    }

    private void EnsureActive()
    {
        if (Status != AccountStatus.Active)
        {
            throw new DomainRuleViolationException(DomainRuleCode.AccountIsInactive);
        }
    }

    private static string Require(string value) =>
        !string.IsNullOrWhiteSpace(value) ? value : throw new DomainRuleViolationException(DomainRuleCode.RequiredValue);
}
