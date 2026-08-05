using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Authentication.Logout;

public sealed record LogoutCommand : ICommand<LogoutResponse>;
