using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
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

        var response = await enrollmentRepository.FirstOrDefaultAsync(
            new EnrollmentResponseByStudentSpec(currentUser.StudentId.Value),
            cancellationToken);

        return response is null
            ? ApplicationResults.NotFound<EnrollmentResponse>(ErrorCode.EnrollmentNotFound)
            : Result<EnrollmentResponse>.Success(response);
    }
}
