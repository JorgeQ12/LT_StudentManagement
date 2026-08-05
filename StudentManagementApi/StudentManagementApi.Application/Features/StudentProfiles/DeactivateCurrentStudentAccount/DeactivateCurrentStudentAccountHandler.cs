using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.StudentProfiles.DeactivateCurrentStudentAccount;

internal sealed class DeactivateCurrentStudentAccountHandler(
    ICurrentUser currentUser,
    IWriteRepository<Student> studentRepository,
    IWriteRepository<Enrollment> enrollmentRepository,
    TimeProvider timeProvider
) : IRequestHandler<DeactivateCurrentStudentAccountCommand, Result<LogoutResponse>>
{
    public async Task<Result<LogoutResponse>> Handle(DeactivateCurrentStudentAccountCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<LogoutResponse>();
        }

        var student = await studentRepository.FirstOrDefaultAsync(new StudentByIdSpec(currentUser.StudentId.Value), cancellationToken);
        if (student is null)
        {
            return ApplicationResults.NotFound<LogoutResponse>(ErrorCode.StudentNotFound);
        }

        var now = timeProvider.GetUtcNow();
        var enrollment = await enrollmentRepository.FirstOrDefaultAsync(new ActiveEnrollmentByStudentSpec(student.Id), cancellationToken);
        if (enrollment is not null)
        {
            enrollment.Cancel(now);
            await enrollmentRepository.UpdateAsync(enrollment, cancellationToken);
        }

        student.Deactivate(now);
        student.Account.Deactivate(now);
        await studentRepository.UpdateAsync(student, cancellationToken);
        return Result<LogoutResponse>.NoContent();
    }
}
