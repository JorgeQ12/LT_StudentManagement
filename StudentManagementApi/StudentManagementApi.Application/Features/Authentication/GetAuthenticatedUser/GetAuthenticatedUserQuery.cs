using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Authentication.GetAuthenticatedUser;

public sealed record GetAuthenticatedUserQuery : IQuery<AuthenticatedUserResponse>;
