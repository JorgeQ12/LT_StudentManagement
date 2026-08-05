using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Courses.AssignProfessor;

public sealed record AssignProfessorCommand(Guid CourseId, Guid ProfessorId) : ICommand<CourseResponse>;
