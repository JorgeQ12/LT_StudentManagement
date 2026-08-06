using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;
using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Features.Administration.Students.GetAllStudents;

public sealed record GetAllStudentsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    AccountStatus? Status = null) : IQuery<PagedResponse<StudentResponse>>;
