using Ardalis.Result;
using MediatR;
using StudentManagementApi.Application.Common.Mappings;
using StudentManagementApi.Application.Common.Persistence;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Application.Specifications;
using StudentManagementApi.Domain.Students;

namespace StudentManagementApi.Application.Features.Administration.Students.GetAllStudents;

internal sealed class GetAllStudentsHandler(IReadRepository<Student> studentRepository)
    : IRequestHandler<GetAllStudentsQuery, Result<PagedResponse<StudentResponse>>>
{
    public async Task<Result<PagedResponse<StudentResponse>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await studentRepository.CountAsync(cancellationToken);
        var students = await studentRepository.ListAsync(new StudentsPageSpec(request.PageNumber, request.PageSize), cancellationToken);
        return Result<PagedResponse<StudentResponse>>.Success(new(students.Select(student => student.ToResponse()).ToArray(), request.PageNumber,
            request.PageSize, totalCount));
    }
}
