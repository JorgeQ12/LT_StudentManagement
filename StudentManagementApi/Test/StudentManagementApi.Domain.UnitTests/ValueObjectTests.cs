using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Domain.UnitTests;

public sealed class ValueObjectTests
{
    [Fact]
    public void ValuesAreNormalizedAtTheBoundary()
    {
        Assert.Equal("student@example.com", Email.Create(" Student@Example.COM ").Value);
        Assert.Equal("CC12345", DocumentNumber.Create(" cc12345 ").Value);
        Assert.Equal("PGU", ProgramCode.Create(" pgu ").Value);
        Assert.Equal("MAT101", CourseCode.Create(" mat101 ").Value);
        Assert.Equal("+57 300 123 4567", PhoneNumber.Create(" +57 300 123 4567 ").Value);
    }

    [Theory]
    [InlineData("invalid", DomainRuleCode.InvalidEmail)]
    [InlineData("a", DomainRuleCode.InvalidDocumentNumber)]
    [InlineData("1", DomainRuleCode.InvalidProgramCode)]
    [InlineData("X", DomainRuleCode.InvalidCourseCode)]
    [InlineData("123", DomainRuleCode.InvalidPhoneNumber)]
    public void InvalidValueIsRejectedWithTechnicalRuleCode(string value, DomainRuleCode expectedCode)
    {
        var exception = Assert.Throws<DomainRuleViolationException>(() => CreateByCode(value, expectedCode));
        Assert.Equal(expectedCode, exception.Code);
    }

    private static void CreateByCode(string value, DomainRuleCode code)
    {
        switch (code)
        {
            case DomainRuleCode.InvalidEmail: _ = Email.Create(value); break;
            case DomainRuleCode.InvalidDocumentNumber: _ = DocumentNumber.Create(value); break;
            case DomainRuleCode.InvalidProgramCode: _ = ProgramCode.Create(value); break;
            case DomainRuleCode.InvalidCourseCode: _ = CourseCode.Create(value); break;
            case DomainRuleCode.InvalidPhoneNumber: _ = PhoneNumber.Create(value); break;
            default: throw new ArgumentOutOfRangeException(nameof(code));
        }
    }
}
