using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Accounts;

namespace StudentManagementApi.Application.Features.Authentication.GetAuthenticatedUser;

internal sealed class GetAuthenticatedUserHandler(ICurrentUser currentUser, IReadRepository<UserAccount> accountRepository)
    : IRequestHandler<GetAuthenticatedUserQuery, Result<AuthenticatedUserResponse>>
{
    public async Task<Result<AuthenticatedUserResponse>> Handle(GetAuthenticatedUserQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.AccountId is null)
        {
            return ApplicationResults.Unauthorized<AuthenticatedUserResponse>();
        }

        var account = await accountRepository.FirstOrDefaultAsync(new AccountByIdSpec(currentUser.AccountId.Value), cancellationToken);

        return account is null
            ? ApplicationResults.Unauthorized<AuthenticatedUserResponse>()
            : Result<AuthenticatedUserResponse>.Success(account.ToAuthenticatedUser());
    }
}
