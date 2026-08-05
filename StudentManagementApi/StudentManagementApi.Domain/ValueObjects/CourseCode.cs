using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.ValueObjects;

public sealed record CourseCode
{
    public const int MaximumLength = 15;
    private CourseCode(string value) => Value = value;
    public string Value { get; }

    public static CourseCode Create(string value)
    {
        var normalized = value?.Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length is < 3 or > MaximumLength)
        {
            throw new DomainRuleViolationException(DomainRuleCode.InvalidCourseCode);
        }

        return new CourseCode(normalized);
    }
}
