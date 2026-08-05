using StudentManagementApi.Application.Common.Messaging;
using StudentManagementApi.Application.Contracts;

namespace StudentManagementApi.Application.Features.StudentProfiles.UpdateCurrentStudentProfile;

public sealed record UpdateCurrentStudentProfileCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email) : ICommand<StudentResponse>;
