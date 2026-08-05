using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Administration.Students.ActivateStudent;

internal sealed class ActivateStudentHandler(IWriteRepository<Student> studentRepository, TimeProvider timeProvider)
    : IRequestHandler<ActivateStudentCommand, Result<StudentResponse>>
{
    public async Task<Result<StudentResponse>> Handle(ActivateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.FirstOrDefaultAsync(new StudentByIdSpec(new(request.StudentId)), cancellationToken);
        if (student is null) return ApplicationResults.NotFound<StudentResponse>(ErrorCode.StudentNotFound);
        var now = timeProvider.GetUtcNow();
        student.Activate(now);
        student.Account.Activate(now);
        await studentRepository.UpdateAsync(student, cancellationToken);
        return Result<StudentResponse>.Success(student.ToResponse());
    }
}
