namespace StudentManagementApi.Presentation.WebApi.Contracts.StudentProfiles;

public sealed record UpdateCurrentStudentProfileRequest(string FirstName, string LastName, DateOnly DateOfBirth, string PhoneNumber, string Email);
