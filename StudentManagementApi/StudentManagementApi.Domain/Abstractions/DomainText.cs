namespace StudentManagementApi.Domain.Abstractions;

internal static class DomainText
{
    public static string Required(string? value, int maximumLength)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new DomainRuleViolationException(DomainRuleCode.RequiredValue);
        var normalized = value.Trim();
        return normalized.Length <= maximumLength
            ? normalized
            : throw new DomainRuleViolationException(DomainRuleCode.ValueExceedsMaximumLength);
    }
}
