namespace StudentManagementApi.Presentation.Lambda.Contracts.Administration.Courses;

public sealed record CreateCourseRequest(Guid AcademicProgramId, string Code, string Name);
