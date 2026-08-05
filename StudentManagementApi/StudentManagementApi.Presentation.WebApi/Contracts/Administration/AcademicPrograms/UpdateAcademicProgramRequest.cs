namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.AcademicPrograms;

public sealed record UpdateAcademicProgramRequest(Guid AcademicProgramId, string Code, string Name, string Description);
