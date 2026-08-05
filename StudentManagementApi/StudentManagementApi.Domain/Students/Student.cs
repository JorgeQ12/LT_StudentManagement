using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Domain.Students;

public sealed class Student : AggregateRoot<StudentId>
{
    public const int MaximumNameLength = 100;
    private Student()
        : base(default) { }

    private Student(
        StudentId id,
        UserAccountId accountId,
        string firstName,
        string lastName,
        DocumentNumber documentNumber,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        DateTimeOffset createdAtUtc
    )
        : base(id)
    {
        AccountId = accountId;
        FirstName = DomainText.Required(firstName, MaximumNameLength);
        LastName = DomainText.Required(lastName, MaximumNameLength);
        DocumentNumber = documentNumber;
        DateOfBirth = ValidateBirthDate(dateOfBirth, createdAtUtc);
        PhoneNumber = phoneNumber;
        Status = AccountStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public UserAccountId AccountId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DocumentNumber DocumentNumber { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public AccountStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }
    public UserAccount Account { get; private set; } = null!;
    public ICollection<Enrollment> Enrollments { get; private set; } = [];

    public static Student Create(
        StudentId id,
        UserAccountId accountId,
        string firstName,
        string lastName,
        DocumentNumber documentNumber,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        DateTimeOffset createdAtUtc
    ) => new(id, accountId, firstName, lastName, documentNumber, dateOfBirth, phoneNumber, createdAtUtc);

    public void UpdateProfile(string firstName, string lastName, DateOnly dateOfBirth, PhoneNumber phoneNumber, DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        FirstName = DomainText.Required(firstName, MaximumNameLength);
        LastName = DomainText.Required(lastName, MaximumNameLength);
        DateOfBirth = ValidateBirthDate(dateOfBirth, updatedAtUtc);
        PhoneNumber = phoneNumber;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void UpdateByAdministrator(string firstName, string lastName, DocumentNumber documentNumber, DateOnly dateOfBirth, PhoneNumber phoneNumber,
        DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        FirstName = DomainText.Required(firstName, MaximumNameLength);
        LastName = DomainText.Required(lastName, MaximumNameLength);
        DocumentNumber = documentNumber;
        DateOfBirth = ValidateBirthDate(dateOfBirth, updatedAtUtc);
        PhoneNumber = phoneNumber;
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

    private static DateOnly ValidateBirthDate(DateOnly value, DateTimeOffset referenceUtc) =>
        value < DateOnly.FromDateTime(referenceUtc.UtcDateTime)
            ? value
            : throw new DomainRuleViolationException(DomainRuleCode.InvalidDateOfBirth);

}
