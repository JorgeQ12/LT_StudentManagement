using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.ValueObjects;

public sealed record ProgramCode
{
    public const int MaximumLength = 15;
    private ProgramCode(string value) => Value = value;
    public string Value { get; }

    public static ProgramCode Create(string value)
    {
        var normalized = value?.Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length is < 2 or > MaximumLength)
        {
            throw new DomainRuleViolationException(DomainRuleCode.InvalidProgramCode);
        }

        return new ProgramCode(normalized);
    }
}
