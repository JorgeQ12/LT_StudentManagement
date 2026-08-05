using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.ValueObjects;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Bootstrap;

public sealed class AdministratorBootstrapper(StudentManagementDbContext dbContext, IPasswordHasher passwordHasher,
    IOptions<AdministratorBootstrapOptions> options, TimeProvider timeProvider)
{
    public async Task CreateIfConfiguredAsync(CancellationToken cancellationToken = default)
    {
        var settings = options.Value;
        if (!settings.IsConfigured) return;

        var email = Email.Create(settings.Email!);
        if (await dbContext.UserAccounts.AnyAsync(account => account.Email == email, cancellationToken)) return;

        var administrator = UserAccount.CreateAdministrator(UserAccountId.New(), email, passwordHasher.Hash(settings.Password!), timeProvider.GetUtcNow());
        dbContext.UserAccounts.Add(administrator);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
