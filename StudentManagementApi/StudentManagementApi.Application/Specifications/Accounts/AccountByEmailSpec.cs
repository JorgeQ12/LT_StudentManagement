using Ardalis.Specification;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AccountByEmailSpec : SingleResultSpecification<UserAccount>
{
    public AccountByEmailSpec(string email)
    {
        var normalizedEmail = Email.Create(email);
        Query.Where(account => account.Email == normalizedEmail);
    }
}
