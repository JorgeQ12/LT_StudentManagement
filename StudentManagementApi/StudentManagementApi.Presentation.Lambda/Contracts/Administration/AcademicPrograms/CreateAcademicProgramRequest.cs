namespace StudentManagementApi.Presentation.Lambda.Contracts.Administration.AcademicPrograms;

public sealed record CreateAcademicProgramRequest(string Code, string Name, string Description);
