namespace StudentManagementApi.Presentation.Lambda.Contracts.Administration.Students;

public sealed record CreateStudentRequest(string FirstName, string LastName, string DocumentNumber, DateOnly DateOfBirth, string PhoneNumber, string Email,
    string Password);
