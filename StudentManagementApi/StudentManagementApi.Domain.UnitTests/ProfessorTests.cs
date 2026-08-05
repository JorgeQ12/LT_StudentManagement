using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Professors;

namespace StudentManagementApi.Domain.UnitTests;

public sealed class ProfessorTests
{
    [Fact]
    public void AssignCourseThirdCourseInSameProgramIsRejected()
    {
        var now = DateTimeOffset.UtcNow;
        var programId = AcademicProgramId.New();
        var professor = Professor.Create(ProfessorId.New(), "Ana", "Martínez", now);
        professor.AssignCourse(TeachingAssignmentId.New(), CourseId.New(), programId, now);
        professor.AssignCourse(TeachingAssignmentId.New(), CourseId.New(), programId, now);

        var exception = Assert.Throws<DomainRuleViolationException>(() => professor.AssignCourse(TeachingAssignmentId.New(), CourseId.New(), programId, now));

        Assert.Equal(DomainRuleCode.ProfessorCourseLimitExceeded, exception.Code);
    }

    [Fact]
    public void AssignCourseTwoCoursesInDifferentProgramsIsAllowed()
    {
        var now = DateTimeOffset.UtcNow;
        var professor = Professor.Create(ProfessorId.New(), "Ana", "Martínez", now);
        professor.AssignCourse(TeachingAssignmentId.New(), CourseId.New(), AcademicProgramId.New(), now);
        professor.AssignCourse(TeachingAssignmentId.New(), CourseId.New(), AcademicProgramId.New(), now);

        Assert.Equal(2, professor.TeachingAssignments.Count);
    }

    [Fact]
    public void ProfessorWithTeachingAssignmentCannotBeDeactivated()
    {
        var now = DateTimeOffset.UtcNow;
        var professor = Professor.Create(ProfessorId.New(), "Ana", "Martínez", now);
        professor.AssignCourse(TeachingAssignmentId.New(), CourseId.New(), AcademicProgramId.New(), now);

        var exception = Assert.Throws<DomainRuleViolationException>(() => professor.Deactivate(now));

        Assert.Equal(DomainRuleCode.CourseAlreadyAssigned, exception.Code);
        Assert.Equal(CatalogStatus.Active, professor.Status);
    }
}
