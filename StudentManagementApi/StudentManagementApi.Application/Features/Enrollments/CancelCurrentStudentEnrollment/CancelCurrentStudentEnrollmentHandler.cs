using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Abstractions;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Features.Enrollments.CancelCurrentStudentEnrollment;

internal sealed class CancelCurrentStudentEnrollmentHandler(
    ICurrentUser currentUser,
    IWriteRepository<Enrollment> enrollmentRepository,
    TimeProvider timeProvider
) : IRequestHandler<CancelCurrentStudentEnrollmentCommand, Result<NoContentResponse>>
{
    public async Task<Result<NoContentResponse>> Handle(CancelCurrentStudentEnrollmentCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<NoContentResponse>();
        }

        var enrollment = await enrollmentRepository.FirstOrDefaultAsync(new ActiveEnrollmentByStudentSpec(currentUser.StudentId.Value), cancellationToken);

        if (enrollment is null)
        {
            return ApplicationResults.NotFound<NoContentResponse>(ErrorCode.EnrollmentNotFound);
        }

        try
        {
            enrollment.Cancel(timeProvider.GetUtcNow());
            await enrollmentRepository.UpdateAsync(enrollment, cancellationToken);
            return Result<NoContentResponse>.NoContent();
        }
        catch (DomainRuleViolationException exception)
        {
            return DomainErrorMapper.ToResult<NoContentResponse>(exception);
        }
    }
}
