using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Application.Common.Security;

public interface IAccessTokenProvider
{
    AccessToken Create(UserAccount account, DateTimeOffset issuedAtUtc);
}
