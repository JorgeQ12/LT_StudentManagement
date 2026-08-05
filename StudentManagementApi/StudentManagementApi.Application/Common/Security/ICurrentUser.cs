using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;

namespace StudentManagementApi.Application.Common.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    UserAccountId? AccountId { get; }
    StudentId? StudentId { get; }
    AccountRole? Role { get; }
}
