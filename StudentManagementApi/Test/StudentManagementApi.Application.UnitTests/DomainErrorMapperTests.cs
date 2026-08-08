using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.UnitTests;

public sealed class DomainErrorMapperTests
{
    [Theory]
    [MemberData(nameof(DomainRules))]
    public void EveryDomainRuleMapsToSpecificApplicationError(DomainRuleCode ruleCode)
    {
        var result = DomainErrorMapper.ToResult<object>(new DomainRuleViolationException(ruleCode));
        var mappedCode = result.ValidationErrors.Any()
            ? Assert.Single(result.ValidationErrors).ErrorCode
            : Assert.Single(result.Errors);

        Assert.NotEqual(nameof(ErrorCode.ValidationFailed), mappedCode);
        Assert.True(Enum.TryParse<ErrorCode>(mappedCode, out _), mappedCode);
    }

    public static TheoryData<DomainRuleCode> DomainRules => new(Enum.GetValues<DomainRuleCode>());
}
