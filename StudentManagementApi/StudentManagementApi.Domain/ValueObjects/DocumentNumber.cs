using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Domain.ValueObjects;

public sealed record DocumentNumber
{
    public const int MaximumLength = 30;
    private DocumentNumber(string value) => Value = value;

    public string Value { get; }
    public string NormalizedValue => Value.ToUpperInvariant();

    public static DocumentNumber Create(string value)
    {
        var normalized = value?.Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Length is < 5 or > MaximumLength)
        {
            throw new DomainRuleViolationException(DomainRuleCode.InvalidDocumentNumber);
        }

        return new DocumentNumber(normalized);
    }
}
