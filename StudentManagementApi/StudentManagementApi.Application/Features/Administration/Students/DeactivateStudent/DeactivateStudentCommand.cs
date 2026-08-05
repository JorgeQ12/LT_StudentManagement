using StudentManagementApi.Application.Common.Messaging;

namespace StudentManagementApi.Application.Features.Administration.Students.DeactivateStudent;

public sealed record DeactivateStudentCommand(Guid StudentId) : ICommand<NoContentResponse>;
