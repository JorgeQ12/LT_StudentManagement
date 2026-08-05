using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Students.ActivateStudent;

public sealed record ActivateStudentCommand(Guid StudentId) : ICommand<StudentResponse>;
