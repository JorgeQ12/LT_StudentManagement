using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.StudentProfiles.DeactivateCurrentStudentAccount;

public sealed record DeactivateCurrentStudentAccountCommand : ICommand<LogoutResponse>;
