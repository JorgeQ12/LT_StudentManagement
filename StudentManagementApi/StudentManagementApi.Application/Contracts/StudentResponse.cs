using StudentManagementApi.Domain;

namespace StudentManagementApi.Application.Contracts;

public sealed record StudentResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string DocumentNumber,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    AccountStatus Status);
