using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Domain.Enrollments;

public sealed class Enrollment : AggregateRoot<EnrollmentId>
{
    public const int RequiredCourseCount = 3;
    public const int RequiredTotalCredits = 9;
    private readonly List<EnrollmentCourse> _courses = [];

    private Enrollment()
        : base(default) { }

    private Enrollment(
        EnrollmentId id,
        StudentId studentId,
        AcademicProgramId academicProgramId,
        IReadOnlyCollection<CourseSelection> selections,
        DateTimeOffset createdAtUtc
    )
        : base(id)
    {
        StudentId = studentId;
        AcademicProgramId = academicProgramId;
        Status = EnrollmentStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
        ReplaceSelectionInternal(selections);
    }

    public StudentId StudentId { get; private set; }
    public AcademicProgramId AcademicProgramId { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public int TotalCredits => _courses.Count * Course.RequiredCredits;
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }
    public DateTimeOffset? CancelledAtUtc { get; private set; }
    public IReadOnlyCollection<EnrollmentCourse> Courses => _courses;
    public Student Student { get; private set; } = null!;
    public AcademicProgram AcademicProgram { get; private set; } = null!;

    public static Enrollment Create(
        EnrollmentId id,
        StudentId studentId,
        AcademicProgramId academicProgramId,
        IReadOnlyCollection<CourseSelection> selections,
        DateTimeOffset createdAtUtc
    ) => new(id, studentId, academicProgramId, selections, createdAtUtc);

    public void ReplaceSelectedCourses(IReadOnlyCollection<CourseSelection> selections, DateTimeOffset updatedAtUtc)
    {
        EnsureActive();
        ReplaceSelectionInternal(selections);
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Cancel(DateTimeOffset cancelledAtUtc)
    {
        EnsureActive();
        Status = EnrollmentStatus.Cancelled;
        CancelledAtUtc = cancelledAtUtc;
        UpdatedAtUtc = cancelledAtUtc;
    }

    private void ReplaceSelectionInternal(IReadOnlyCollection<CourseSelection> selections)
    {
        ValidateSelections(selections);
        _courses.Clear();
        _courses.AddRange(selections.Select(selection => EnrollmentCourse.Create(EnrollmentCourseId.New(), Id, selection.CourseId)));
    }

    private void ValidateSelections(IReadOnlyCollection<CourseSelection> selections)
    {
        if (selections.Count != RequiredCourseCount)
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentMustContainThreeCourses);
        }

        if (selections.Select(item => item.CourseId).Distinct().Count() != RequiredCourseCount)
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentCoursesMustBeDistinct);
        }

        if (selections.Any(item => item.AcademicProgramId != AcademicProgramId))
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentCoursesMustBelongToSameProgram);
        }

        if (selections.Any(item => item.CourseStatus != CatalogStatus.Active || item.ProfessorStatus != CatalogStatus.Active))
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentCoursesMustBeActive);
        }

        if (selections.Select(item => item.ProfessorId).Distinct().Count() != RequiredCourseCount)
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentCoursesMustHaveDifferentProfessors);
        }

        if (selections.Sum(item => item.Credits) != RequiredTotalCredits)
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentMustContainThreeCourses);
        }
    }

    private void EnsureActive()
    {
        if (Status != EnrollmentStatus.Active)
        {
            throw new DomainRuleViolationException(DomainRuleCode.EnrollmentIsNotActive);
        }
    }
}
