using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.Common.Security;

public interface IAccountSessionValidator
{
    Task<bool> IsActiveAsync(UserAccountId accountId, CancellationToken cancellationToken);
}
