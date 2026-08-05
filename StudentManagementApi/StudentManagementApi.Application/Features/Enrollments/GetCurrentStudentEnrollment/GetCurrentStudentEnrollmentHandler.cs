using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Enrollments;

namespace StudentManagementApi.Application.Features.Enrollments.GetCurrentStudentEnrollment;

internal sealed class GetCurrentStudentEnrollmentHandler(ICurrentUser currentUser, IReadRepository<Enrollment> enrollmentRepository)
    : IRequestHandler<GetCurrentStudentEnrollmentQuery, Result<EnrollmentResponse>>
{
    public async Task<Result<EnrollmentResponse>> Handle(GetCurrentStudentEnrollmentQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<EnrollmentResponse>();
        }

        var enrollment = await enrollmentRepository.FirstOrDefaultAsync(new ActiveEnrollmentByStudentSpec(currentUser.StudentId.Value), cancellationToken);

        return enrollment is null
            ? ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.EnrollmentNotFound)
            : Result<EnrollmentResponse>.Success(enrollment.ToResponse());
    }
}
