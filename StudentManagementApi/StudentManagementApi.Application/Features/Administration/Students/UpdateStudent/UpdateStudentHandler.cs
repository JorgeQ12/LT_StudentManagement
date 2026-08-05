using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Accounts;
using StudentManagementApi.Domain.Students;
using StudentManagementApi.Domain.ValueObjects;

namespace StudentManagementApi.Application.Features.Administration.Students.UpdateStudent;

internal sealed class UpdateStudentHandler(IWriteRepository<Student> studentRepository, IWriteRepository<UserAccount> accountRepository,
    TimeProvider timeProvider) : IRequestHandler<UpdateStudentCommand, Result<StudentResponse>>
{
    public async Task<Result<StudentResponse>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var studentId = new StudentId(request.StudentId);
        var student = await studentRepository.FirstOrDefaultAsync(new StudentByIdSpec(studentId), cancellationToken);
        if (student is null) return ApplicationResults.NotFound<StudentResponse>(ErrorCode.StudentNotFound);

        var documentOwner = await studentRepository.FirstOrDefaultAsync(new StudentByDocumentSpec(request.DocumentNumber), cancellationToken);
        if (documentOwner is not null && documentOwner.Id != studentId)
            return ApplicationResults.Conflict<StudentResponse>(ErrorCode.DocumentNumberAlreadyExists);
        var emailOwner = await accountRepository.FirstOrDefaultAsync(new AccountByEmailSpec(request.Email), cancellationToken);
        if (emailOwner is not null && emailOwner.Id != student.AccountId)
            return ApplicationResults.Conflict<StudentResponse>(ErrorCode.EmailAlreadyExists);

        try
        {
            var now = timeProvider.GetUtcNow();
            student.UpdateByAdministrator(request.FirstName, request.LastName, DocumentNumber.Create(request.DocumentNumber), request.DateOfBirth,
                PhoneNumber.Create(request.PhoneNumber), now);
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
