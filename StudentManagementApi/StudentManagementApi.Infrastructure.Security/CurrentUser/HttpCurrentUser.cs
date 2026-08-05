using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Infrastructure.Security.Jwt;

namespace StudentManagementApi.Infrastructure.Security.CurrentUser;

internal sealed class HttpCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;
    public UserAccountId? AccountId => TryGetGuid(SecurityClaimTypes.Subject, out var value) ? new(value) : null;
    public StudentId? StudentId => TryGetGuid(SecurityClaimTypes.StudentId, out var value) ? new(value) : null;
    public AccountRole? Role => Enum.TryParse<AccountRole>(Principal?.FindFirstValue(SecurityClaimTypes.Role), out var role) ? role : null;

    private bool TryGetGuid(string claimType, out Guid value) => Guid.TryParse(Principal?.FindFirstValue(claimType), out value);
}
