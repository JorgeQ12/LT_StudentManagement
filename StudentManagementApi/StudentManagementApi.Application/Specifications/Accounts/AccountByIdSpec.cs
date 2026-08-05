using Ardalis.Specification;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AccountByIdSpec : SingleResultSpecification<UserAccount>
{
    public AccountByIdSpec(UserAccountId id) => Query.Where(account => account.Id == id);
}
