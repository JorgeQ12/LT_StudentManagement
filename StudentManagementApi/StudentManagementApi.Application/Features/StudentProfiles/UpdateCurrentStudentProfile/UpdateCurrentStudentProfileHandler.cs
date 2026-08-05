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

namespace StudentManagementApi.Application.Features.StudentProfiles.UpdateCurrentStudentProfile;

internal sealed class UpdateCurrentStudentProfileHandler(
    ICurrentUser currentUser,
    IWriteRepository<Student> studentRepository,
    IWriteRepository<UserAccount> accountRepository,
    TimeProvider timeProvider
) : IRequestHandler<UpdateCurrentStudentProfileCommand, Result<StudentResponse>>
{
    public async Task<Result<StudentResponse>> Handle(UpdateCurrentStudentProfileCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<StudentResponse>();
        }

        var student = await studentRepository.FirstOrDefaultAsync(new StudentByIdSpec(currentUser.StudentId.Value), cancellationToken);
        if (student is null)
        {
            return ApplicationResults.NotFound<StudentResponse>(ErrorCode.StudentNotFound);
        }

        var existingAccount = await accountRepository.FirstOrDefaultAsync(new AccountByEmailSpec(request.Email), cancellationToken);
        if (existingAccount is not null && existingAccount.Id != student.AccountId)
        {
            return ApplicationResults.Conflict<StudentResponse>(ErrorCode.EmailAlreadyExists);
        }

        try
        {
            var now = timeProvider.GetUtcNow();
            student.UpdateProfile(request.FirstName, request.LastName, request.DateOfBirth, PhoneNumber.Create(request.PhoneNumber), now);
            student.Account.UpdateEmail(Email.Create(request.Email), now);
            await studentRepository.UpdateAsync(student, cancellationToken);
            return Result<StudentResponse>.Success(student.ToResponse());
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<StudentResponse>(exception);
        }
    }
}
