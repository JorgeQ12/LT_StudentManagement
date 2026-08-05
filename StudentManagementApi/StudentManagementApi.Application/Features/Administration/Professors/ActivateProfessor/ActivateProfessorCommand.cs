using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Professors.ActivateProfessor;

public sealed record ActivateProfessorCommand(Guid ProfessorId) : ICommand<ProfessorResponse>;
