using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Domain.Programs;

public sealed class AcademicProgram : AggregateRoot<AcademicProgramId>
{
    public const int MaximumNameLength = 150;
    public const int MaximumDescriptionLength = 500;
    private AcademicProgram()
        : base(default) { }

    private AcademicProgram(AcademicProgramId id, ProgramCode code, string name, string description, DateTimeOffset createdAtUtc)
        : base(id)
    {
        Code = code;
        Name = DomainText.Required(name, MaximumNameLength);
        Description = DomainText.Required(description, MaximumDescriptionLength);
        Status = CatalogStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public ProgramCode Code { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public CatalogStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static AcademicProgram Create(
        AcademicProgramId id,
        ProgramCode code,
        string name,
        string description,
        DateTimeOffset createdAtUtc
    ) => new(id, code, name, description, createdAtUtc);

    public void Update(ProgramCode code, string name, string description, DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        Code = code;
        Name = DomainText.Required(name, MaximumNameLength);
        Description = DomainText.Required(description, MaximumDescriptionLength);
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Activate(DateTimeOffset updatedAtUtc)
    {
        Status = CatalogStatus.Active;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Deactivate(DateTimeOffset updatedAtUtc)
    {
        Status = CatalogStatus.Inactive;
        UpdatedAtUtc = updatedAtUtc;
    }

    private void EnsureActive()
    {
        if (Status != CatalogStatus.Active)
        {
            throw new DomainRuleViolationException(DomainRuleCode.CatalogItemIsInactive);
        }
    }

}
