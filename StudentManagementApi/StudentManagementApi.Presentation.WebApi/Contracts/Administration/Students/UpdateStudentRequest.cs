namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Students;

public sealed record UpdateStudentRequest(Guid StudentId, string FirstName, string LastName, string DocumentNumber, DateOnly DateOfBirth, string PhoneNumber,
    string Email);
