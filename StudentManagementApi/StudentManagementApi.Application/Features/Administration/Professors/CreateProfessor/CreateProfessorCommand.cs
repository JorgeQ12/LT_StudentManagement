using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Professors.CreateProfessor;

public sealed record CreateProfessorCommand(string FirstName, string LastName) : ICommand<ProfessorResponse>;
