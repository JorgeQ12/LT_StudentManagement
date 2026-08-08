using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Common.Security;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.StudentProfiles.GetCurrentStudentProfile;

internal sealed class GetCurrentStudentProfileHandler(ICurrentUser currentUser, IReadRepository<Student> studentRepository)
    : IRequestHandler<GetCurrentStudentProfileQuery, Result<StudentResponse>>
{
    public async Task<Result<StudentResponse>> Handle(GetCurrentStudentProfileQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.StudentId is null)
        {
            return ApplicationResults.Forbidden<StudentResponse>();
        }

        var response = await studentRepository.FirstOrDefaultAsync(
            new StudentResponseByIdSpec(currentUser.StudentId.Value),
            cancellationToken);

        return response is null
            ? ApplicationResults.NotFound<StudentResponse>(ErrorCode.StudentNotFound)
            : Result<StudentResponse>.Success(response);
    }
}
