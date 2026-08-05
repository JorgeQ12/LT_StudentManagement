using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Enrollments;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Administration.Students.DeactivateStudent;

internal sealed class DeactivateStudentHandler(IWriteRepository<Student> studentRepository, IWriteRepository<Enrollment> enrollmentRepository,
    TimeProvider timeProvider) : IRequestHandler<DeactivateStudentCommand, Result<NoContentResponse>>
{
    public async Task<Result<NoContentResponse>> Handle(DeactivateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.FirstOrDefaultAsync(new StudentByIdSpec(new(request.StudentId)), cancellationToken);
        if (student is null) return ApplicationResults.NotFound<NoContentResponse>(ErrorCode.StudentNotFound);
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
        return Result<NoContentResponse>.NoContent();
    }
}
