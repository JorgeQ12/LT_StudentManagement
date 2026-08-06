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
    public async Task<Result<PagedResponse<StudentResponse>>> Handle(
        GetAllStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();

        var totalCount = await studentRepository.CountAsync(
            new StudentsCountSpec(search, request.Status),
            cancellationToken);
        var students = await studentRepository.ListAsync(
            new StudentsPageSpec(request.PageNumber, request.PageSize, search, request.Status),
            cancellationToken);

        return Result<PagedResponse<StudentResponse>>.Success(new(
            students.Select(student => student.ToResponse()).ToArray(),
            request.PageNumber,
            request.PageSize,
            totalCount));
    }
}
