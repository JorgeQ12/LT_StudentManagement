using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.Administration.Students.UpdateStudent;

public sealed record UpdateStudentCommand(Guid StudentId, string FirstName, string LastName, string DocumentNumber, DateOnly DateOfBirth, string PhoneNumber,
    string Email) : ICommand<StudentResponse>;
