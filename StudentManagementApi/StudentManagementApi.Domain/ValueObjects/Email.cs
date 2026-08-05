using System.Text.RegularExpressions;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.ValueObjects;

public sealed partial record Email
{
    public const int MaximumLength = 254;
    private Email(string value) => Value = value;

    public string Value { get; }
    public string NormalizedValue => Value.ToUpperInvariant();

    public static Email Create(string value)
    {
        var normalized = value?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length > MaximumLength || !EmailPattern().IsMatch(normalized))
        {
            throw new DomainRuleViolationException(DomainRuleCode.InvalidEmail);
        }

        return new Email(normalized);
    }

    [GeneratedRegex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.CultureInvariant)]
    private static partial Regex EmailPattern();
}
