namespace StudentManagementApi.Application.Common.Security;

public sealed record AccessToken(string Value, DateTimeOffset ExpiresAtUtc);
