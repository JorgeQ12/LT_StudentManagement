using Ardalis.Specification;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Programs;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.UnitTests;

public sealed class SpecificationTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 5, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void ActiveAcademicProgramsFiltersOrdersAndProjectsWithoutTracking()
    {
        var inactive = CreateProgram("ZZZ", "Zulu");
        inactive.Deactivate(Now);
        var programs = new[] { inactive, CreateProgram("BBB", "Beta"), CreateProgram("AAA", "Alpha") };
        var specification = new ActiveAcademicProgramsSpec();

        var result = InMemorySpecificationEvaluator.Default.Evaluate(programs, specification).ToArray();

        Assert.Equal(["Alpha", "Beta"], result.Select(program => program.Name));
        Assert.True(specification.AsNoTracking);
    }

    [Fact]
    public void AcademicProgramsOrdersCatalogWithoutTracking()
    {
        var programs = new[] { CreateProgram("ZZZ", "Zulu"), CreateProgram("AAA", "Alpha") };
        var specification = new AcademicProgramsSpec();

        var result = InMemorySpecificationEvaluator.Default.Evaluate(programs, specification).ToArray();

        Assert.Equal(["Alpha", "Zulu"], result.Select(program => program.Name));
        Assert.True(specification.AsNoTracking);
    }

    private static AcademicProgram CreateProgram(string code, string name) =>
        AcademicProgram.Create(AcademicProgramId.New(), ProgramCode.Create(code), name, $"{name} description", Now);
}
