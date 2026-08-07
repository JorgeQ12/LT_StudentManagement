namespace StudentManagementApi.Presentation.Lambda.Contracts.Authentication;

public sealed record RegisterStudentRequest(string FirstName, string LastName, string DocumentNumber, DateOnly DateOfBirth, string PhoneNumber, string Email,
    string Password);
