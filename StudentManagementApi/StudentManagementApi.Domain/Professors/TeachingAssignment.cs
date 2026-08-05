using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;

namespace StudentManagementApi.Domain.Professors;

public sealed class TeachingAssignment : Entity<TeachingAssignmentId>
{
    private TeachingAssignment()
        : base(default) { }

    private TeachingAssignment(
        TeachingAssignmentId id,
        ProfessorId professorId,
        CourseId courseId,
        AcademicProgramId academicProgramId,
        DateTimeOffset assignedAtUtc
    )
        : base(id)
    {
        ProfessorId = professorId;
        CourseId = courseId;
        AcademicProgramId = academicProgramId;
        AssignedAtUtc = assignedAtUtc;
    }

    public ProfessorId ProfessorId { get; private set; }
    public CourseId CourseId { get; private set; }
    public AcademicProgramId AcademicProgramId { get; private set; }
    public DateTimeOffset AssignedAtUtc { get; private set; }
    public Professor Professor { get; private set; } = null!;
    public Course Course { get; private set; } = null!;

    internal static TeachingAssignment Create(
        TeachingAssignmentId id,
        ProfessorId professorId,
        CourseId courseId,
        AcademicProgramId academicProgramId,
        DateTimeOffset assignedAtUtc
    ) => new(id, professorId, courseId, academicProgramId, assignedAtUtc);
}
