namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Courses;

public sealed record CreateCourseRequest(Guid AcademicProgramId, string Code, string Name);
