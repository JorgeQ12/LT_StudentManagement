using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Students.GetAllStudents;

public sealed record GetAllStudentsQuery(int PageNumber = 1, int PageSize = 20) : IQuery<PagedResponse<StudentResponse>>;
