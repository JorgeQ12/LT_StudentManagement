using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.Specifications;

internal static class SearchValueObject
{
    public static string? Normalize(string? search) =>
        string.IsNullOrWhiteSpace(search) ? null : search.Trim();

    public static T? TryCreate<T>(string? search, Func<string, T> factory)
        where T : class
    {
        if (search is null)
        {
            return null;
        }

        try
        {
            return factory(search);
        }
        catch (DomainRuleViolationException)
        {
            return null;
        }
    }
}
