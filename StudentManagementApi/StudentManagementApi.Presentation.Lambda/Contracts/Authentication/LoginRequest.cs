namespace StudentManagementApi.Presentation.Lambda.Contracts.Authentication;

public sealed record LoginRequest(string Email, string Password);
