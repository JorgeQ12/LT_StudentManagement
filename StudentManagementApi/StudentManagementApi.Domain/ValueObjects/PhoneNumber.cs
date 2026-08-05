using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.ValueObjects;

public sealed record PhoneNumber
{
    public const int MaximumLength = 30;
    private PhoneNumber(string value) => Value = value;

    public string Value { get; }

    public static PhoneNumber Create(string value)
    {
        var normalized = value?.Trim();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length > MaximumLength || normalized.Count(char.IsDigit) is < 7 or > 15)
        {
            throw new DomainRuleViolationException(DomainRuleCode.InvalidPhoneNumber);
        }

        return new PhoneNumber(normalized);
    }
}
