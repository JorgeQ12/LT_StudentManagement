using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Courses;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Domain.UnitTests;

public sealed class CatalogStateTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 5, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void CourseAlwaysHasExactlyThreeCredits()
    {
        var course = Course.Create(CourseId.New(), AcademicProgramId.New(), CourseCode.Create("MAT101"), "Mathematics", Now);
        Assert.Equal(3, course.Credits);
    }

    [Fact]
    public void InactiveCourseCannotBeUpdatedUntilReactivated()
    {
        var course = Course.Create(CourseId.New(), AcademicProgramId.New(), CourseCode.Create("MAT101"), "Mathematics", Now);
        course.Deactivate(Now.AddMinutes(1));

        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            course.Update(CourseCode.Create("MAT102"), "Advanced mathematics", Now.AddMinutes(2)));
        Assert.Equal(DomainRuleCode.CatalogItemIsInactive, exception.Code);

        course.Activate(Now.AddMinutes(3));
        course.Update(CourseCode.Create("MAT102"), "Advanced mathematics", Now.AddMinutes(4));
        Assert.Equal("MAT102", course.Code.Value);
    }

    [Fact]
    public void InactiveAcademicProgramCannotBeUpdated()
    {
        var program = AcademicProgram.Create(AcademicProgramId.New(), ProgramCode.Create("PGU"), "General", "Description", Now);
        program.Deactivate(Now.AddMinutes(1));

        var exception = Assert.Throws<DomainRuleViolationException>(() =>
            program.Update(ProgramCode.Create("PGU"), "Updated", "Updated description", Now.AddMinutes(2)));
        Assert.Equal(DomainRuleCode.CatalogItemIsInactive, exception.Code);
    }
}
