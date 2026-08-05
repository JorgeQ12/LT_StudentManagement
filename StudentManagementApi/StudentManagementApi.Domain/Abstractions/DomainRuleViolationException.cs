namespace StudentManagementApi.Domain.Abstractions;

public sealed class DomainRuleViolationException(DomainRuleCode code) : Exception(code.ToString())
{
    public DomainRuleCode Code { get; } = code;
}
