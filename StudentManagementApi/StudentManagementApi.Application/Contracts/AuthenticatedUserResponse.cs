using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Contracts;

public sealed record AuthenticatedUserResponse(Guid AccountId, Guid? StudentId, string Email, AccountRole Role);
