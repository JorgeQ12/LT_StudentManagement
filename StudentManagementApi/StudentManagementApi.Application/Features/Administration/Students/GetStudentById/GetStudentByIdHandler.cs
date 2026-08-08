using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Errors;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Administration.Students.GetStudentById;

internal sealed class GetStudentByIdHandler(IReadRepository<Student> studentRepository) : IRequestHandler<GetStudentByIdQuery, Result<StudentResponse>>
{
    public async Task<Result<StudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await studentRepository.FirstOrDefaultAsync(
            new StudentResponseByIdSpec(new(request.StudentId)),
            cancellationToken);

        return response is null
            ? ApplicationResults.NotFound<StudentResponse>(ErrorCode.StudentNotFound)
            : Result<StudentResponse>.Success(response);
    }
}
