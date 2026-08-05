using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain;
using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Application.Features.Authentication.Login;

internal sealed class LoginHandler(
    IWriteRepository<UserAccount> accountRepository,
    IPasswordHasher passwordHasher,
    IAccessTokenProvider accessTokenProvider,
    TimeProvider timeProvider
) : IRequestHandler<LoginCommand, Result<AuthenticationSessionResponse>>
{
    public async Task<Result<AuthenticationSessionResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.FirstOrDefaultAsync(new AccountByEmailSpec(request.Email), cancellationToken);

        if (account is null || !passwordHasher.Verify(account.PasswordHash, request.Password))
        {
            return ApplicationResults.Unauthorized<AuthenticationSessionResponse>(ErrorCode.InvalidCredentials);
        }

        if (account.Status != AccountStatus.Active)
        {
            return ApplicationResults.Forbidden<AuthenticationSessionResponse>(ErrorCode.AccountInactive);
        }

        var token = accessTokenProvider.Create(account, timeProvider.GetUtcNow());
        return Result<AuthenticationSessionResponse>.Success(new(account.ToAuthenticatedUser(), token.Value, token.ExpiresAtUtc));
    }
}
