namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Professors;

public sealed record UpdateProfessorRequest(Guid ProfessorId, string FirstName, string LastName);
