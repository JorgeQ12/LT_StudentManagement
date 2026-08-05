using StudentManagementApi.Application.Common.Messaging;

namespace StudentManagementApi.Application.Features.Administration.Professors.DeactivateProfessor;

public sealed record DeactivateProfessorCommand(Guid ProfessorId) : ICommand<NoContentResponse>;
