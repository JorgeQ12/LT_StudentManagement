namespace StudentManagementApi.Presentation.WebApi.Contracts.Administration.Courses;

public sealed record AssignProfessorRequest(Guid CourseId, Guid ProfessorId);
