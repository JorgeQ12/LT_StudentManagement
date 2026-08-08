using Ardalis.Specification;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Application.Specifications;

internal sealed class AuthenticatedUserResponseByIdSpec
    : SingleResultSpecification<UserAccount, AuthenticatedUserResponse>
{
    public AuthenticatedUserResponseByIdSpec(UserAccountId accountId) =>
        Query.Where(account => account.Id == accountId)
            .AsNoTracking()
            .Select(account => new AuthenticatedUserResponse(
                account.Id.Value,
                account.StudentId.HasValue ? account.StudentId.Value.Value : null,
                account.Email.Value,
                account.Role));
}
