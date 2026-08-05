using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Authentication.RegisterStudent;

public sealed record RegisterStudentCommand(
    string FirstName,
    string LastName,
    string DocumentNumber,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    string Password) : ICommand<AuthenticationSessionResponse>;
