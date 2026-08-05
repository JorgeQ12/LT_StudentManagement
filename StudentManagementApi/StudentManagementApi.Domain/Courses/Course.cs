using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Domain.Courses;

public sealed class Course : AggregateRoot<CourseId>
{
    public const int RequiredCredits = 3;
    public const int MaximumNameLength = 150;

    private Course()
        : base(default) { }

    private Course(CourseId id, AcademicProgramId academicProgramId, CourseCode code, string name, DateTimeOffset createdAtUtc)
        : base(id)
    {
        AcademicProgramId = academicProgramId;
        Code = code;
        Name = DomainText.Required(name, MaximumNameLength);
        Credits = RequiredCredits;
        Status = CatalogStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public AcademicProgramId AcademicProgramId { get; private set; }
    public CourseCode Code { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public int Credits { get; private set; }
    public CatalogStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }
    public AcademicProgram AcademicProgram { get; private set; } = null!;
    public TeachingAssignment? TeachingAssignment { get; private set; }

    public static Course Create(
        CourseId id,
        AcademicProgramId academicProgramId,
        CourseCode code,
        string name,
        DateTimeOffset createdAtUtc
    ) => new(id, academicProgramId, code, name, createdAtUtc);

    public void Update(CourseCode code, string name, DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        Code = code;
        Name = DomainText.Required(name, MaximumNameLength);
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
