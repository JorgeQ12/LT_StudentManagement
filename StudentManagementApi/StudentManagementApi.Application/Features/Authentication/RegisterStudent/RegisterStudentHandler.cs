using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.Students;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Authentication.RegisterStudent;

internal sealed class RegisterStudentHandler(
    IWriteRepository<UserAccount> accountRepository,
    IWriteRepository<Student> studentRepository,
    IPasswordHasher passwordHasher,
    IAccessTokenProvider accessTokenProvider,
    TimeProvider timeProvider
) : IRequestHandler<RegisterStudentCommand, Result<AuthenticationSessionResponse>>
{
    public async Task<Result<AuthenticationSessionResponse>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        if (await accountRepository.AnyAsync(new AccountByEmailSpec(request.Email), cancellationToken))
        {
            return ApplicationResults.Conflict<AuthenticationSessionResponse>(ErrorCode.EmailAlreadyExists);
        }

        if (await studentRepository.AnyAsync(new StudentByDocumentSpec(request.DocumentNumber), cancellationToken))
        {
            return ApplicationResults.Conflict<AuthenticationSessionResponse>(ErrorCode.DocumentNumberAlreadyExists);
        }

        try
        {
            var now = timeProvider.GetUtcNow();
            var accountId = UserAccountId.New();
            var studentId = StudentId.New();
            var account = UserAccount.CreateStudent(accountId, studentId, Email.Create(request.Email), passwordHasher.Hash(request.Password), now);
            var student = Student.Create(
                studentId,
                accountId,
                request.FirstName,
                request.LastName,
                DocumentNumber.Create(request.DocumentNumber),
                request.DateOfBirth,
                PhoneNumber.Create(request.PhoneNumber),
                now
            );

            await accountRepository.AddAsync(account, cancellationToken);
            await studentRepository.AddAsync(student, cancellationToken);

            var token = accessTokenProvider.Create(account, now);
            return Result<AuthenticationSessionResponse>.Created(new(account.ToAuthenticatedUser(), token.Value, token.ExpiresAtUtc));
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<AuthenticationSessionResponse>(exception);
        }
    }
}
