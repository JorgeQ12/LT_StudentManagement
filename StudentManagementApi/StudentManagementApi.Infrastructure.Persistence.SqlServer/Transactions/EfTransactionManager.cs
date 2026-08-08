using System.Data;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Infrastructure.Persistence.SqlServer.Context;

namespace StudentManagementApi.Infrastructure.Persistence.SqlServer.Transactions;

internal sealed class EfTransactionManager(StudentManagementWriteDbContext dbContext) : ITransactionManager
{
    public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken)
    {
        if (dbContext.Database.CurrentTransaction is not null)
        {
            return await operation(cancellationToken);
        }

        var strategy = dbContext.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var result = await operation(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        });
    }
}
