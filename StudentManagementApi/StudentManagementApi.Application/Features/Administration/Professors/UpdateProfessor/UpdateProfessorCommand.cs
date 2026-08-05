using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Professors.UpdateProfessor;

public sealed record UpdateProfessorCommand(Guid ProfessorId, string FirstName, string LastName) : ICommand<ProfessorResponse>;
