namespace StudentManagementApi.Application.Common.Persistence;

public interface ITransactionManager
{
    Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken);
}
