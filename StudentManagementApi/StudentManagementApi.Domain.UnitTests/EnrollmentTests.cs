using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Domain.UnitTests;

public sealed class EnrollmentTests
{
    private static readonly AcademicProgramId ProgramId = AcademicProgramId.New();

    [Fact]
    public void CreateWithThreeCoursesAndDifferentProfessorsCreatesNineCreditEnrollment()
    {
        var enrollment = Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, ValidSelections(), DateTimeOffset.UtcNow);

        Assert.Equal(EnrollmentStatus.Active, enrollment.Status);
        Assert.Equal(3, enrollment.Courses.Count);
        Assert.Equal(9, enrollment.TotalCredits);
    }

    [Fact]
    public void CreateWithTwoCoursesRejectsSelection()
    {
        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, ValidSelections()[..2], DateTimeOffset.UtcNow)
        );

        Assert.Equal(DomainRuleCode.EnrollmentMustContainThreeCourses, exception.Code);
    }

    [Fact]
    public void CreateWithRepeatedProfessorRejectsSelection()
    {
        var professorId = ProfessorId.New();
        var selections = ValidSelections().Select(item => item with { ProfessorId = professorId }).ToArray();

        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, selections, DateTimeOffset.UtcNow)
        );

        Assert.Equal(DomainRuleCode.EnrollmentCoursesMustHaveDifferentProfessors, exception.Code);
    }

    [Fact]
    public void CreateWithRepeatedCourseRejectsSelection()
    {
        var courseId = CourseId.New();
        var selections = ValidSelections().Select(item => item with { CourseId = courseId }).ToArray();

        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, selections, DateTimeOffset.UtcNow));

        Assert.Equal(DomainRuleCode.EnrollmentCoursesMustBeDistinct, exception.Code);
    }

    [Fact]
    public void CreateWithCourseFromAnotherProgramRejectsSelection()
    {
        var selections = ValidSelections();
        selections[0] = selections[0] with { AcademicProgramId = AcademicProgramId.New() };

        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, selections, DateTimeOffset.UtcNow));

        Assert.Equal(DomainRuleCode.EnrollmentCoursesMustBelongToSameProgram, exception.Code);
    }

    [Fact]
    public void CreateWithInactiveCourseRejectsSelection()
    {
        var selections = ValidSelections();
        selections[0] = selections[0] with { CourseStatus = CatalogStatus.Inactive };

        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, selections, DateTimeOffset.UtcNow));

        Assert.Equal(DomainRuleCode.EnrollmentCoursesMustBeActive, exception.Code);
    }

    [Fact]
    public void CancelledEnrollmentCannotBeChangedOrCancelledTwice()
    {
        var enrollment = Enrollment.Create(EnrollmentId.New(), StudentId.New(), ProgramId, ValidSelections(), DateTimeOffset.UtcNow);
        enrollment.Cancel(DateTimeOffset.UtcNow);

        var replaceException = Assert.Throws<DomainRuleViolationException>(() =>
            enrollment.ReplaceSelectedCourses(ValidSelections(), DateTimeOffset.UtcNow));
        var cancelException = Assert.Throws<DomainRuleViolationException>(() => enrollment.Cancel(DateTimeOffset.UtcNow));

        Assert.Equal(EnrollmentStatus.Cancelled, enrollment.Status);
        Assert.Equal(DomainRuleCode.EnrollmentIsNotActive, replaceException.Code);
        Assert.Equal(DomainRuleCode.EnrollmentIsNotActive, cancelException.Code);
    }

    private static CourseSelection[] ValidSelections() => [Selection(), Selection(), Selection()];

    private static CourseSelection Selection() =>
        new(CourseId.New(), ProgramId, ProfessorId.New(), CatalogStatus.Active, CatalogStatus.Active, Course.RequiredCredits);
}
