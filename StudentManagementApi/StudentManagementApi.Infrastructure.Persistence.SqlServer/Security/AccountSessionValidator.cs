using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Domain;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Security;

internal sealed class AccountSessionValidator(StudentManagementReadDbContext dbContext) : IAccountSessionValidator
{
    public Task<bool> IsActiveAsync(UserAccountId accountId, CancellationToken cancellationToken) =>
        dbContext.Set<UserAccount>().AnyAsync(account => account.Id == accountId && account.Status == AccountStatus.Active, cancellationToken);
}
