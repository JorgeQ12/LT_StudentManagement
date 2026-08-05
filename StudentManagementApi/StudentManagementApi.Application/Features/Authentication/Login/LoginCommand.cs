using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Authentication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<AuthenticationSessionResponse>;
