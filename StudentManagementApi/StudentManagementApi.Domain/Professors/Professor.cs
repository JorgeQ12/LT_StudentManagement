using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.Professors;

public sealed class Professor : AggregateRoot<ProfessorId>
{
    public const int MaximumCoursesPerProgram = 2;
    public const int MaximumNameLength = 100;
    private readonly List<TeachingAssignment> _teachingAssignments = [];

    private Professor()
        : base(default) { }

    private Professor(ProfessorId id, string firstName, string lastName, DateTimeOffset createdAtUtc)
        : base(id)
    {
        FirstName = DomainText.Required(firstName, MaximumNameLength);
        LastName = DomainText.Required(lastName, MaximumNameLength);
        Status = CatalogStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public CatalogStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }
    public IReadOnlyCollection<TeachingAssignment> TeachingAssignments => _teachingAssignments;

    public static Professor Create(ProfessorId id, string firstName, string lastName, DateTimeOffset createdAtUtc) =>
        new(id, firstName, lastName, createdAtUtc);

    public TeachingAssignment AssignCourse(
        TeachingAssignmentId assignmentId,
        CourseId courseId,
        AcademicProgramId academicProgramId,
        DateTimeOffset updatedAtUtc
    )
    {
        EnsureActive();

        if (_teachingAssignments.Any(item => item.CourseId == courseId))
        {
            throw new DomainRuleViolationException(DomainRuleCode.CourseAlreadyAssigned);
        }

        if (_teachingAssignments.Count(item => item.AcademicProgramId == academicProgramId) >= MaximumCoursesPerProgram)
        {
            throw new DomainRuleViolationException(DomainRuleCode.ProfessorCourseLimitExceeded);
        }

        var assignment = TeachingAssignment.Create(assignmentId, Id, courseId, academicProgramId, updatedAtUtc);

        _teachingAssignments.Add(assignment);
        UpdatedAtUtc = updatedAtUtc;
        return assignment;
    }

    public void RemoveCourse(CourseId courseId, DateTimeOffset updatedAtUtc)
    {
        var assignment =
            _teachingAssignments.SingleOrDefault(item => item.CourseId == courseId)
            ?? throw new DomainRuleViolationException(DomainRuleCode.TeachingAssignmentNotFound);

        _teachingAssignments.Remove(assignment);
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Update(string firstName, string lastName, DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        FirstName = DomainText.Required(firstName, MaximumNameLength);
        LastName = DomainText.Required(lastName, MaximumNameLength);
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Activate(DateTimeOffset updatedAtUtc)
    {
        Status = CatalogStatus.Active;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Deactivate(DateTimeOffset updatedAtUtc)
    {
        if (_teachingAssignments.Count != 0)
        {
            throw new DomainRuleViolationException(DomainRuleCode.CourseAlreadyAssigned);
        }

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
