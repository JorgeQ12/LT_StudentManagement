using Microsoft.AspNetCore.Identity;
using StudentManagementApi.Application.Common.Security;

namespace StudentManagementApi.Infrastructure.Security.Passwords;

internal sealed class AspNetPasswordHasher : IPasswordHasher
{
    private static readonly object UserContext = new();
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password) => _passwordHasher.HashPassword(UserContext, password);

    public bool Verify(string passwordHash, string password) =>
        _passwordHasher.VerifyHashedPassword(UserContext, passwordHash, password) != PasswordVerificationResult.Failed;
}
