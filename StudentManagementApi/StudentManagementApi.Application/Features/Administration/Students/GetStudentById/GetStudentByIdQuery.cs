using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Students.GetStudentById;

public sealed record GetStudentByIdQuery(Guid StudentId) : IQuery<StudentResponse>;
