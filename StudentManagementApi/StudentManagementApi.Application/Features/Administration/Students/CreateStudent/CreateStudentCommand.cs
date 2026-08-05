using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Students.CreateStudent;

public sealed record CreateStudentCommand(string FirstName, string LastName, string DocumentNumber, DateOnly DateOfBirth, string PhoneNumber, string Email,
    string Password) : ICommand<StudentResponse>;
