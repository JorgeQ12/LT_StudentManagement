namespace StudentManagementApi.Presentation.Lambda.Contracts.Administration.Professors;

public sealed record UpdateProfessorRequest(Guid ProfessorId, string FirstName, string LastName);
